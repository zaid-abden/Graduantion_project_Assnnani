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

namespace GraduationProject.Application.Features.Receptionists.Commands.CompleteAppointment
{
    //public class CompleteAppointmentCommandHandler
    //: IRequestHandler<CompleteAppointmentCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork unitOfWork;

    //    public CompleteAppointmentCommandHandler(IUnitOfWork unitOfWork)
    //    {
    //        this.unitOfWork = unitOfWork;
    //    }

    //    public async Task<Result<string>> Handle(
    //        CompleteAppointmentCommand request,
    //        CancellationToken cancellationToken)
    //    {
    //        var appointment = await unitOfWork.Appointments.Query()
    //            .FirstOrDefaultAsync(x =>
    //                x.AppointmentId == request.AppointmentId
    //                && !x.IsDeleted,
    //                cancellationToken);

    //        if (appointment is null)
    //            return Result<string>.Failure(ResultStatus.NotFound, "Appointment not found");


    //        if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Cannot complete cancelled appointment");

    //        if (appointment.AppointmentStatus == AppointmentStatus.Completed)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Appointment already completed");


    //        if (appointment.QueueStatus != QueueStatus.InProgress)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Appointment must be in progress to complete");


    //        appointment.AppointmentStatus = AppointmentStatus.Completed;




    //        appointment.QueueStatus = QueueStatus.Complete;

    //        await unitOfWork.SaveAsync();

    //        return Result<string>.Success("Appointment completed successfully");
    //    }
    //}
    public class CompleteAppointmentCommandHandler
       : IRequestHandler<CompleteAppointmentCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        private readonly INotificationService _notificationService;

        public CompleteAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser,INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            this._notificationService = notificationService;
        }

        public async Task<Result<string>> Handle(
            CompleteAppointmentCommand request,
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
                    "Receptionist account not found."
                );

            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.AppointmentId == request.AppointmentId
                         && !x.IsDeleted,
                    cancellationToken);

            if (appointment is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Appointment not found."
                );

          
            if (appointment.DoctorId != receptionist.DoctorId)
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to manage this appointment."
                );

            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Cannot complete a cancelled appointment."
                );

            if (appointment.AppointmentStatus == AppointmentStatus.Completed)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "This appointment has already been completed."
                );

            if (appointment.QueueStatus != QueueStatus.InProgress)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Appointment must be in progress before completion."
                );

            appointment.AppointmentStatus = AppointmentStatus.Completed;
            appointment.QueueStatus = QueueStatus.Complete;


          
            await _unitOfWork.SaveAsync();


            await _notificationService.SendToUserAsync(
  appointment.Doctor.UserId,
  "Appointment Completed",
  $"Appointment with {appointment.Patient.User.FullName} has been completed.",
  NotificationType.Success
);



            await _notificationService.SendToUserAsync(
                appointment.Patient.UserId,
                "Appointment Completed",
                "Your appointment has been completed. Thank you for visiting.",
                NotificationType.Success
            );

            return Result<string>.Success(
                "Appointment completed successfully."
            );
        }
    }
}
