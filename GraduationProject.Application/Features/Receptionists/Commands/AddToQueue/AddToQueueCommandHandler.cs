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

namespace GraduationProject.Application.Features.Receptionists.Commands.AddToQueue
{
    public class AddToQueueCommandHandler
    : IRequestHandler<AddToQueueCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;

        public AddToQueueCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService
            ,INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            this._notificationService = notificationService;
        }

        public async Task<Result<string>> Handle(
            AddToQueueCommand request,
            CancellationToken cancellationToken)
        {
          
            if (!_currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized");

            var userId = _currentUserService.UserId;

            
            var doctorId = await _unitOfWork.Receptionists.Query()
                .Where(x => x.UserId == userId)
                .Select(x => x.DoctorId)
                .FirstOrDefaultAsync(cancellationToken);
            var doctorUserId = await _unitOfWork.Doctors.Query()
                .Where(x => x.DoctorId == doctorId)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(cancellationToken);
            if (doctorId == 0)
                return Result<string>.Failure(ResultStatus.NotFound, "Receptionist not assigned to doctor");

           
            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.ScheduleSlot)
                .Include(x => x.Patient)
                .ThenInclude(x => x.User)
                .Include(x => x.Doctor)
                
                .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Appointment not found");

          
            if (appointment.DoctorId != doctorId)
                return Result<string>.Failure(ResultStatus.Forbidden, "You cannot manage this appointment");

           
            if (!appointment.IsCheckedIn)
                return Result<string>.Failure(ResultStatus.Conflict, "Patient must check-in first");

          
            if (appointment.QueueStatus != QueueStatus.Arrived)
                return Result<string>.Failure(ResultStatus.Conflict, "Patient is not ready for queue");

            var today = DateOnly.FromDateTime(DateTime.Today);

            
            var lastQueueNumber = await _unitOfWork.Appointments.Query()
                .Where(x =>
                    x.DoctorId == doctorId &&
                    x.ScheduleSlot.Date == today &&
                    x.QueueNumber != null)
                .MaxAsync(x => (int?)x.QueueNumber, cancellationToken) ?? 0;

            appointment.QueueNumber = lastQueueNumber + 1;
            appointment.QueueStatus = QueueStatus.Waiting;

            await _unitOfWork.SaveAsync();
            await _notificationService.SendToUserAsync(
    doctorUserId!,
    "New Patient in Queue",
    $"Patient {appointment.Patient.User.FullName} has been added to your queue.",
    NotificationType.Info
);
            await _notificationService.SendToUserAsync(
    appointment.Patient.UserId,
    "Queue Update",
    $"You have been added to Dr. {appointment.Doctor.FullName}'s queue.",
    NotificationType.Success
);


            return Result<string>.Success($"Added to queue with number {appointment.QueueNumber}");
        }
    }
}
