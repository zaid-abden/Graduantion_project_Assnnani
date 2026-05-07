using GraduationProject.Application.BackgroundJobs.Appointments;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Data.Enums;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, Result<AddAppointmentResponseDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RescheduleAppointmentCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<AddAppointmentResponseDto>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Unauthorized, "You must be logged in to book an appointment.");

            var userId = currentUserService.UserId;

            var patientId = await unitOfWork.Patients.Query()
                .Where(x => x.UserId == userId)
                .Select(x => x.PatientId)
                .FirstOrDefaultAsync(cancellationToken);

            var appointment = await unitOfWork.Appointments.Query()
      .Include(x => x.ScheduleSlot)
      .ThenInclude(x => x.DoctorSchedule)
      .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment is null)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.NotFound,"Appointment not found");
            if (appointment.PatientId != patientId)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Forbidden, "You are not allowed to reschedule this appointment");
            var newSlot = await unitOfWork.ScheduleSlots.Query()
    .Include(x => x.DoctorSchedule)
    .FirstOrDefaultAsync(x => x.Id == request.NewScheduleSlotId, cancellationToken);

            if (newSlot is null)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.NotFound, "New slot not found");
            if (newSlot.Status != SlotStatus.Available)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Conflict,"Selected slot is not available");
            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Conflict, "Cannot reschedule cancelled appointment.");
            var hasConflict = await unitOfWork.Appointments.Query()
    .AnyAsync(x =>
        x.PatientId == patientId &&
        x.DoctorId == appointment.DoctorId &&
        x.AppointmentId != appointment.AppointmentId &&
        (x.AppointmentStatus == AppointmentStatus.Pending ||
         x.AppointmentStatus == AppointmentStatus.Confirmed),
        cancellationToken);





            if (hasConflict)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Conflict,"You already have an active appointment with this doctor");
            appointment.ScheduleSlot.Status = SlotStatus.Available;
            appointment.ScheduleSlotId = newSlot.Id;
            appointment.DoctorId = newSlot.DoctorSchedule.DoctorId;

            newSlot.Status = SlotStatus.Booked;
            appointment.AppointmentStatus = AppointmentStatus.Pending;
            var newDateTime = newSlot.DoctorSchedule.Date.ToDateTime(newSlot.StartTime);
            await unitOfWork.SaveAsync();

            BackgroundJob.Schedule<IAppointmentJobService>(
  x => x.AutoConfirm(appointment.AppointmentId),
  TimeSpan.FromMinutes(10));

            return Result<AddAppointmentResponseDto>.Success(new AddAppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                ScheduleSlotId = newSlot.Id,
                AppointmentStatus = appointment.AppointmentStatus,
                BookingType = appointment.BookingType,
                CreatedAt = appointment.CreatedAt,
                AppointmentTime = newSlot.DoctorSchedule.Date.ToDateTime(newSlot.StartTime),
                Message = "Appointment rescheduled successfully"
            });

        }
    }
}
