using GraduationProject.Application.BackgroundJobs.Appointments;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.AddAppointment
{
    public class AddAppointmentCommandHandler : IRequestHandler<AddAppointmentCommand, Result<AddAppointmentResponseDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public AddAppointmentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<AddAppointmentResponseDto>> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Unauthorized, "You must be logged in to book an appointment.");

            var userId = currentUserService.UserId;

            var patient = await unitOfWork.Patients.Query()
                .Where(x => x.UserId == userId)
               
                .FirstOrDefaultAsync(cancellationToken);

            if (patient is null)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.NotFound, "Patient profile not found for the current user.");
            var slot = await unitOfWork.ScheduleSlots.Query()
                .Include(x => x.DoctorSchedule)
                .FirstOrDefaultAsync(x => x.Id == request.ScheduleSlotId, cancellationToken);

            if (slot is null)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.NotFound, "The selected schedule slot does not exist.");
            if (slot.Status == SlotStatus.Booked)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Conflict, "The selected time slot is already booked");

            if (slot.Status == SlotStatus.Blocked)
                return Result<AddAppointmentResponseDto>.Failure(ResultStatus.Conflict, "Sorry, this time slot is currently unavailable due to doctor or clinic unavailability. Please choose another time.");

           
            var doctorId = await unitOfWork.Doctors.Query()
                .Where(x => x.DoctorId == slot.DoctorSchedule.DoctorId)
                .Select(x => x.DoctorId)
                .FirstOrDefaultAsync(cancellationToken);

            var hasActiveAppointment = await unitOfWork.Appointments.Query()
    .AnyAsync(x =>
        x.PatientId == patient.PatientId &&
        x.DoctorId == slot.DoctorSchedule.DoctorId &&
        (x.AppointmentStatus == AppointmentStatus.Pending
         || x.AppointmentStatus == AppointmentStatus.Confirmed),
        cancellationToken);

            if (hasActiveAppointment)
            {
                return Result<AddAppointmentResponseDto>.Failure(
                    ResultStatus.Conflict,
                    "You already have an active appointment with this doctor. Please wait until it is completed or cancel it before booking a new one.");
            }

            var schedule = await unitOfWork.DoctorSchedules.Query()
                .FirstOrDefaultAsync(x => x.ScheduleId == slot.DoctorScheduleId, cancellationToken);
            if (schedule == null)
                return Result<AddAppointmentResponseDto>.Failure(
                   ResultStatus.NotFound,
                   "Schedule not found");

            var dailyCount = await unitOfWork.Appointments.Query()
        .CountAsync(x =>
            x.DoctorId == doctorId &&
            x.ScheduleSlot.DoctorScheduleId == schedule.ScheduleId &&
            (x.AppointmentStatus == AppointmentStatus.Pending ||
             x.AppointmentStatus == AppointmentStatus.Confirmed),
            cancellationToken);


           
            patient.AssignedDoctorId = doctorId;

            if (patient.Status == PatientStatus.Pending)
            {
                patient.Status = PatientStatus.Active;
            }

            if (patient.Status == PatientStatus.InActive)
            {
                patient.Status = PatientStatus.Active;
            }



            var appointment = new Appointment
            {
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                ScheduleSlotId = request.ScheduleSlotId,
                Notes = request.Notes,
                PatientId = patient.PatientId,
                AppointmentStatus = AppointmentStatus.Pending,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,
                DoctorId = doctorId,
                QueueStatus = null,
                IsCheckedIn = false,
                ArrivedAt = null,
               
            };
        

            if(request.appointmentType.HasValue)
                appointment.AppointmentType = request.appointmentType.Value;
                else
                    appointment.AppointmentType = AppointmentType.Emergency;


            if (currentUserService.IsInRole("Patient"))
                appointment.BookingType = BookingType.Online;

            else if (currentUserService.IsInRole("Receptionist"))
                appointment.BookingType = BookingType.PhoneCall;

            await unitOfWork.Appointments.AddAsync(appointment);
            slot.Status = SlotStatus.Booked;
            await unitOfWork.SaveAsync();


            var appointmentDateTime = slot.DoctorSchedule.Date.ToDateTime(slot.StartTime);

            var addAppointmentResponseDto = new AddAppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                ScheduleSlotId = appointment.ScheduleSlotId,
                AppointmentStatus = appointment.AppointmentStatus,
                BookingType = appointment.BookingType,
                CreatedAt = appointment.CreatedAt,
                AppointmentTime = appointmentDateTime,
              
                Message = "Your appointment has been booked successfully"
            };
            patient.AssignedDoctorId = doctorId;
            BackgroundJob.Schedule<IAppointmentJobService>(
    x => x.AutoConfirm(appointment.AppointmentId),
    TimeSpan.FromMinutes(10));

            return Result<AddAppointmentResponseDto>.Success(addAppointmentResponseDto);

        }
    }
}
