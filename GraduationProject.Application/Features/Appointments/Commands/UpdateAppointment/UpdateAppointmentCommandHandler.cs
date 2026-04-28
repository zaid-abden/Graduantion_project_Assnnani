using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.UpdateAppointment
{

    public class UpdateAppointmentCommandHandler
    : IRequestHandler<UpdateAppointmentCommand, Result<AddAppointmentResponseDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<AddAppointmentResponseDto>> Handle(
            UpdateAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<AddAppointmentResponseDto>.Failure(
                    ResultStatus.Unauthorized,
                    "You must be logged in.");

            var userId = currentUserService.UserId;

            var patientId = await unitOfWork.Patients.Query()
                .Where(x => x.UserId == userId)
                .Select(x => x.PatientId)
                .FirstOrDefaultAsync(cancellationToken);

            if (patientId == 0)
                return Result<AddAppointmentResponseDto>.Failure(
                    ResultStatus.NotFound,
                    "Patient not found.");

            var appointment = await unitOfWork.Appointments.Query()
                .Include(x => x.ScheduleSlot)
                    .ThenInclude(x => x.DoctorSchedule)
                .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId,
                    cancellationToken);

            if (appointment is null)
                return Result<AddAppointmentResponseDto>.Failure(
                    ResultStatus.NotFound,
                    "Appointment not found.");

            if (appointment.PatientId != patientId)
                return Result<AddAppointmentResponseDto>.Failure(
                    ResultStatus.Forbidden,
                    "You cannot edit this appointment.");

         
            if (appointment.ScheduleSlotId != request.ScheduleSlotId)
            {
                var newSlot = await unitOfWork.ScheduleSlots.Query()
                    .FirstOrDefaultAsync(x => x.Id == request.ScheduleSlotId,
                        cancellationToken);

                if (newSlot is null)
                    return Result<AddAppointmentResponseDto>.Failure(
                        ResultStatus.NotFound,
                        "New slot not found.");

                if (newSlot.Status != SlotStatus.Available)
                    return Result<AddAppointmentResponseDto>.Failure(
                        ResultStatus.Conflict,
                        "Selected slot is not available.");


                var oldSlot = appointment.ScheduleSlot;
                oldSlot.Status = SlotStatus.Available;


                newSlot.Status = SlotStatus.Booked;

                appointment.ScheduleSlotId = request.ScheduleSlotId;
            }

            appointment.Notes = request.Notes;
            appointment.PaymentMethod = request.PaymentMethod;
            appointment.UpdatedAt = DateTime.Now;
            appointment.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            var slot = await unitOfWork.ScheduleSlots.Query()
                .Include(x => x.DoctorSchedule)
              
                .FirstOrDefaultAsync(x => x.Id == appointment.ScheduleSlotId);
            if(slot is null)
                return Result<AddAppointmentResponseDto>.Failure(
      ResultStatus.NotFound,
      "Schedule slot not found.");
            var response = new AddAppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                ScheduleSlotId = appointment.ScheduleSlotId,
                AppointmentStatus = appointment.AppointmentStatus,
                BookingType = appointment.BookingType,
                CreatedAt = appointment.CreatedAt,
                AppointmentTime = slot.DoctorSchedule.Date.ToDateTime(slot.StartTime),
                Message = "Appointment updated successfully"
            };

            return Result<AddAppointmentResponseDto>.Success(response);
        }
    }
}
