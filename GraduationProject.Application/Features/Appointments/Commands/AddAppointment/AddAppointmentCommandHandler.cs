using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.AddAppointment
{
    public class AddAppointmentCommandHandler : IRequestHandler<AddAppointmentCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public AddAppointmentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "You must be logged in to book an appointment.");

            var userId = currentUserService.UserId;

            var patientId = await unitOfWork.Patients.Query()
                .Where(x => x.UserId == userId)
                .Select(x => x.PatientId)
                .FirstOrDefaultAsync(cancellationToken);

            if (patientId == 0)
                return Result<int>.Failure(ResultStatus.NotFound, "Patient profile not found for the current user.");
            var slot = await unitOfWork.ScheduleSlots.Query()
                .FirstOrDefaultAsync(x => x.Id == request.ScheduleSlotId, cancellationToken);

            if (slot is null)
                return Result<int>.Failure(ResultStatus.NotFound, "The selected schedule slot does not exist.");
            if (slot.Status == SlotStatus.Booked)
                return Result<int>.Failure(ResultStatus.Conflict, "The selected time slot is already booked");

            if (slot.Status == SlotStatus.Blocked)
                return Result<int>.Failure(ResultStatus.Conflict, "This time slot is blocked and cannot be booked.");

            if (slot.Status == SlotStatus.Cancelled)
                return Result<int>.Failure(ResultStatus.Conflict, "This slot has been cancelled and cannot be booked.");



            var appointment = new Appointment
            {
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                ScheduleSlotId = request.ScheduleSlotId,
                Notes = request.Notes,
                PatientId = patientId,
                AppointmentStatus = AppointmentStatus.Pending,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,
               
            };

            if (currentUserService.IsInRole("Patient"))
                appointment.BookingType = BookingType.Online;

            else if (currentUserService.IsInRole("Receptionist"))
                appointment.BookingType = BookingType.WalkIn;

            await unitOfWork.Appointments.AddAsync(appointment);
            slot.Status = SlotStatus.Booked;
            await unitOfWork.SaveAsync();

            return Result<int>.Success(appointment.AppointmentId);

        }
    }
}
