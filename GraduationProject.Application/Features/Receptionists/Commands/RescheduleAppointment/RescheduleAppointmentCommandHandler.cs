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

namespace GraduationProject.Application.Features.Receptionists.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommandHandler
        : IRequestHandler<RescheduleAppointmentCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationService _notificationService;

        public RescheduleAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            this._notificationService = notificationService;
        }

        public async Task<Result<string>> Handle(
            RescheduleAppointmentCommand request,
            CancellationToken cancellationToken)
        {
           
            if (!_currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized");

            var userId = _currentUserService.UserId;

            
            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Receptionist not found");

          
            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.ScheduleSlot)
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.AppointmentId == request.AppointmentId &&
                    x.DoctorId == receptionist.DoctorId,
                    cancellationToken);

            if (appointment is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Appointment not found");

           
            if (appointment.AppointmentStatus == AppointmentStatus.Completed)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot reschedule completed appointment");

            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot reschedule cancelled appointment");

            if (appointment.QueueStatus == QueueStatus.InProgress)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot reschedule while consultation is in progress");

            
            var newSlot = await _unitOfWork.ScheduleSlots.Query()
                .Include(x => x.DoctorSchedule)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.NewSlotId &&
                    x.DoctorSchedule.DoctorId == receptionist.DoctorId,
                    cancellationToken);

            if (newSlot is null)
                return Result<string>.Failure(ResultStatus.NotFound, "New slot not found");

            if (newSlot.Status != SlotStatus.Available)
                return Result<string>.Failure(ResultStatus.Conflict, "New slot is not available");

           
            var alreadyBooked = await _unitOfWork.Appointments.Query()
                .AnyAsync(x =>
                    x.ScheduleSlotId == newSlot.Id &&
                    x.AppointmentStatus != AppointmentStatus.Cancelled,
                    cancellationToken);

            if (alreadyBooked)
                return Result<string>.Failure(ResultStatus.Conflict, "Slot already booked");

           
            var now = TimeOnly.FromDateTime(DateTime.Now);

            if (appointment.ScheduleSlot.Date == DateOnly.FromDateTime(DateTime.Today) &&
                appointment.ScheduleSlot.StartTime <= now)
            {
                return Result<string>.Failure(ResultStatus.Conflict,
                    "Cannot reschedule after appointment has started");
            }

         
            var oldSlot = appointment.ScheduleSlot;
            oldSlot.Status = SlotStatus.Available;

            
            appointment.ScheduleSlotId = newSlot.Id;
            newSlot.Status = SlotStatus.Booked;



        

            await _unitOfWork.SaveAsync();
            await _notificationService.SendToUserAsync(
appointment.Doctor.UserId,
"Appointment Rescheduled",
$"Appointment with {appointment.Patient.User.FullName} has been rescheduled.",
NotificationType.Warning
);



            await _notificationService.SendToUserAsync(
                appointment.Patient.UserId,
                "Appointment Rescheduled",
                $"Your appointment has been rescheduled to {request.NewDate}.",
                NotificationType.Warning
            );

            return Result<string>.Success("Appointment rescheduled successfully");
        }
    }
}
