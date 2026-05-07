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

namespace GraduationProject.Application.Features.Receptionists.Commands.StartConsultation
{
    public class StartConsultationCommandHandler
     : IRequestHandler<StartConsultationCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;

        public StartConsultationCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService
            ,INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            this._notificationService = notificationService;
        }

        public async Task<Result<string>> Handle(
            StartConsultationCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized");

            var userId = _currentUserService.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<string>.Failure(ResultStatus.Forbidden, "You are not authorized to perform this action.");

            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.ScheduleSlot)
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId,
                    cancellationToken);

            if (appointment is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Appointment not found");

           
            if (appointment.DoctorId != receptionist.DoctorId)
                return Result<string>.Failure(ResultStatus.Conflict, "Not allowed for this doctor");

            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot start cancelled appointment");

            if (appointment.AppointmentStatus == AppointmentStatus.Completed)
                return Result<string>.Failure(ResultStatus.Conflict, "Appointment already completed");

            if (!appointment.IsCheckedIn)
                return Result<string>.Failure(ResultStatus.Conflict, "Patient must check-in first");

            if (appointment.QueueStatus != QueueStatus.Waiting)
                return Result<string>.Failure(ResultStatus.Conflict, "Patient is not in waiting queue");

            if (appointment.QueueNumber == null)
                return Result<string>.Failure(ResultStatus.Conflict, "Patient is not in queue");

          
            var hasActiveConsultation = await _unitOfWork.Appointments.Query()
                .AnyAsync(x =>
                    x.DoctorId == receptionist.DoctorId &&
                    x.QueueStatus == QueueStatus.InProgress,
                    cancellationToken);

            if (hasActiveConsultation)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Doctor is already in consultation with another patient");

            appointment.QueueStatus = QueueStatus.InProgress;





            await _unitOfWork.SaveAsync();

            await _notificationService.SendToUserAsync(
    appointment.Doctor.UserId,
    "Consultation Started",
    $"Consultation started with {appointment.Patient.User.FullName}.",
    NotificationType.Info
);

            await _notificationService.SendToUserAsync(
                appointment.Patient.UserId,
                "Consultation Started",
                "Your consultation has started.",
                NotificationType.Info
            );

            return Result<string>.Success("Consultation started successfully");
        }
    }
}
