using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.ConfirmAppointment
{
    public class ConfirmAppointmentCommandHandler
       : IRequestHandler<ConfirmAppointmentCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly INotificationService _notificationService;

        public ConfirmAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser
            ,INotificationService _notificationService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            this._notificationService = _notificationService;
        }

        public async Task<Result<string>> Handle(
            ConfirmAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Access denied. Please login to continue."
                );

            var receptionistUserId = _currentUser.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(
                    x => x.UserId == receptionistUserId,
                    cancellationToken);

            if (receptionist is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Receptionist not found."
                );

            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(
                    a => a.AppointmentId == request.AppointmentId,
                    cancellationToken);

            if (appointment is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Appointment not found."
                );

           
            if (appointment.DoctorId != receptionist.DoctorId)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not allowed to manage appointments for this doctor."
                );

            if (appointment.AppointmentStatus == AppointmentStatus.Confirmed)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Appointment is already confirmed."
                );

            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Cancelled appointments cannot be confirmed."
                );

            if (appointment.AppointmentStatus != AppointmentStatus.Pending)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Only pending appointments can be confirmed."
                );

            appointment.AppointmentStatus = AppointmentStatus.Confirmed;



        

            await _unitOfWork.SaveAsync();
            await _notificationService.SendToUserAsync(
appointment.Doctor.UserId,
"New Confirmed Appointment",
$"You have a new confirmed appointment with {appointment.Patient.User.FullName}.",
NotificationType.Info
);

            await _notificationService.SendToUserAsync(
                appointment.Patient.UserId,
                "Appointment Confirmed",
                "Your appointment has been confirmed successfully.",
                NotificationType.Success
            );

            return Result<string>.Success(
                "Appointment confirmed successfully."
            );
        }
    }
}
