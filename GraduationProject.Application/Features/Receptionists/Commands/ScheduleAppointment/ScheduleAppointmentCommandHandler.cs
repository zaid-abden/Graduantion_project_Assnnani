using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.ScheduleAppointment
{
    //public class ScheduleAppointmentCommandHandler : IRequestHandler<ScheduleAppointmentCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public ScheduleAppointmentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }
    //    public async Task<Result<string>> Handle(ScheduleAppointmentCommand request, CancellationToken cancellationToken)
    //    {
    //        if (!currentUserService.IsAuthenticated)
    //            return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
    //        var userId = currentUserService.UserId;
    //        var doctorId = await unitOfWork.Receptionists.Query()
    //            .Where(c => c.UserId == userId)
    //            .Select(c => c.DoctorId)
    //            .FirstOrDefaultAsync(cancellationToken);
    //        var patient = await unitOfWork.Patients.Query()
    //            .FirstOrDefaultAsync(x => x.PatientId == request.PatientId);
    //        if (patient is null)
    //            return Result<string>.Failure(ResultStatus.NotFound, "Patient profile not found");
    //        var slot = await unitOfWork.ScheduleSlots.Query()
    //             .FirstOrDefaultAsync(x => x.DoctorSchedule.DoctorId == doctorId
    //             && x.Id == request.slotId,cancellationToken);
    //        if (slot is null)
    //            return Result<string>.Failure(ResultStatus.NotFound, "slot not found");
    //        slot.Status = SlotStatus.Booked;

    //        var appointment = new Appointment
    //        {
    //            ScheduleSlotId = slot.Id,
    //            CreatedAt = DateTime.Now,
    //            CreatedBy = currentUserService.UserName,
    //            AppointmentType = request.AppointmentType,
    //            BookingType = BookingType.WalkIn,
    //            AppointmentStatus = AppointmentStatus.Confirmed,
    //            ArrivedAt = TimeOnly.Parse("16:00:00"),
    //            DoctorId = doctorId,
    //            PaymentMethod = request.PaymentMethod,
    //            PatientId = request.PatientId,
    //            ArrivelTime = TimeOnly.Parse("15:45:00"),
    //            Notes = request.Reason,
    //            PatientStatus = PatientStatus.Active,
    //            QueueStatus = QueueStatus.Waiting,
    //            IsCheckedIn = true,
    //            PaymentStatus = PaymentStatus.Pending

    //        };

    //        await unitOfWork.Appointments.AddAsync(appointment);
    //        await unitOfWork.SaveAsync();
    //        return Result<string>.Success("Appointment booked successfully");
    //    }
    //}
    //public class ScheduleAppointmentCommandHandler
    //   : IRequestHandler<ScheduleAppointmentCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public ScheduleAppointmentCommandHandler(
    //        IUnitOfWork unitOfWork,
    //        ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }

    //    public async Task<Result<string>> Handle(
    //        ScheduleAppointmentCommand request,
    //        CancellationToken cancellationToken)
    //    {

    //        if (!currentUserService.IsAuthenticated)
    //            return Result<string>.Failure(
    //                ResultStatus.Unauthorized,
    //                "Unauthorized access");

    //        var userId = currentUserService.UserId;

    //        var now = TimeOnly.FromDateTime(DateTime.Now);
    //        var doctorId = await unitOfWork.Receptionists.Query()
    //            .Where(c => c.UserId == userId)
    //            .Select(c => c.DoctorId)
    //            .FirstOrDefaultAsync(cancellationToken);

    //        if (doctorId == 0)
    //            return Result<string>.Failure(
    //                ResultStatus.NotFound,
    //                "Doctor not found");


    //        var patient = await unitOfWork.Patients.Query()
    //            .FirstOrDefaultAsync(x => x.PatientId == request.PatientId, cancellationToken);

    //        if (patient is null)
    //            return Result<string>.Failure(
    //                ResultStatus.NotFound,
    //                "Patient not found");


    //        var slot = await unitOfWork.ScheduleSlots.Query()
    //            .Include(x => x.DoctorSchedule)
    //            .FirstOrDefaultAsync(x =>
    //                x.Id == request.slotId &&
    //                x.DoctorSchedule.DoctorId == doctorId,
    //                cancellationToken);

    //        if (slot is null)
    //            return Result<string>.Failure(
    //                ResultStatus.NotFound,
    //                "Slot not found");


    //        if (slot.Status != SlotStatus.Available)
    //            return Result<string>.Failure(
    //                ResultStatus.Conflict,
    //                "Slot is no longer available");


    //        var isAlreadyBooked = await unitOfWork.Appointments.Query()
    //            .AnyAsync(a =>
    //                a.ScheduleSlotId == slot.Id &&
    //                a.AppointmentStatus != AppointmentStatus.Cancelled,
    //                cancellationToken);

    //        if (isAlreadyBooked)
    //            return Result<string>.Failure(
    //                ResultStatus.Conflict,
    //                "Slot already booked");

    //        if (now > slot.StartTime)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Cannot book a past time slot");




    //        slot.Status = SlotStatus.Booked;



    //        patient.AssignedDoctorId = doctorId;
    //        var appointment = new Appointment
    //        {
    //            ScheduleSlotId = slot.Id,

    //            DoctorId = doctorId,
    //            PatientId = request.PatientId,

    //            CreatedAt = DateTime.Now,
    //            CreatedBy = currentUserService.UserName,

    //            AppointmentType = request.AppointmentType,
    //            BookingType = BookingType.WalkIn,

    //            AppointmentStatus = AppointmentStatus.Confirmed,


    //            IsCheckedIn = true,
    //            ArrivedAt = now,
    //            ArrivelTime = now,
    //            QueueStatus = QueueStatus.Waiting,

    //            Notes = request.Reason,


    //            PaymentMethod = request.PaymentMethod,
    //            PaymentStatus = PaymentStatus.Pending,

    //            PatientStatus = PatientStatus.Active
    //        };

    //        await unitOfWork.Appointments.AddAsync(appointment);
    //        await unitOfWork.SaveAsync();

    //        return Result<string>.Success("Appointment booked successfully");
    //    }
    //}
    public class ScheduleAppointmentCommandHandler
    : IRequestHandler<ScheduleAppointmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly INotificationService _notificationService;

        public ScheduleAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,INotificationService notificationService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this._notificationService = notificationService;
        }

        public async Task<Result<string>> Handle(
            ScheduleAppointmentCommand request,
            CancellationToken cancellationToken)
        {

            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access");

            var userId = currentUserService.UserId;

            var now = TimeOnly.FromDateTime(DateTime.Now);
            var today = DateOnly.FromDateTime(DateTime.Today);


            var doctorId = await unitOfWork.Receptionists.Query()
                .Where(c => c.UserId == userId)
                .Select(c => c.DoctorId)
                .FirstOrDefaultAsync(cancellationToken);

            if (doctorId == 0)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor not found");
            var doctor = await unitOfWork.Doctors.Query()
                .Where(x => x.DoctorId == doctorId)
                
                .FirstOrDefaultAsync(cancellationToken);
            if(doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor not found");
            var patient = await unitOfWork.Patients.Query()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId, cancellationToken);

            if (patient is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Patient not found");


            var slot = await unitOfWork.ScheduleSlots.Query()
                .Include(x => x.DoctorSchedule)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.slotId &&
                    x.DoctorSchedule.DoctorId == doctorId,
                    cancellationToken);

            if (slot is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Slot not found");


            if (slot.Date < today)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot book past date");


            if (slot.Date == today && now > slot.StartTime)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot book a past time slot");


            if (slot.Status != SlotStatus.Available)
                return Result<string>.Failure(ResultStatus.Conflict, "Slot is no longer available");


            var isAlreadyBooked = await unitOfWork.Appointments.Query()
                .AnyAsync(a =>
                    a.ScheduleSlotId == slot.Id &&
                    a.AppointmentStatus != AppointmentStatus.Cancelled,
                    cancellationToken);

            if (isAlreadyBooked)
                return Result<string>.Failure(ResultStatus.Conflict, "Slot already booked");


            slot.Status = SlotStatus.Booked;


            if (patient.AssignedDoctorId == null)
                patient.AssignedDoctorId = doctorId;

            patient.Status = PatientStatus.Active;








            var appointment = new Appointment
            {
                ScheduleSlotId = slot.Id,

                DoctorId = doctorId,
                PatientId = request.PatientId,

                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,

                AppointmentType = request.AppointmentType,


                AppointmentStatus = AppointmentStatus.Confirmed,




                Notes = request.Reason,


                PaymentMethod = request.PaymentMethod,
                PaymentStatus = PaymentStatus.Pending,

                PatientStatus = PatientStatus.Active
            };


            if (request.BookingType == BookingType.WalkIn)
            {
                appointment.IsCheckedIn = true;
                appointment.ArrivedAt = now;
                appointment.ArrivelTime = now;
                appointment.QueueStatus = QueueStatus.Arrived;
            }
            else if (request.BookingType == BookingType.Online)
            {
                appointment.IsCheckedIn = false;
                appointment.ArrivedAt = null;

                appointment.QueueStatus = null;
            }
            appointment.BookingType = request.BookingType;
            await unitOfWork.Appointments.AddAsync(appointment);

           

            await unitOfWork.SaveAsync();
            await _notificationService.SendToUserAsync(
   doctor.UserId,
   "New Appointment Scheduled",
   $"You have a new appointment with {patient.User.FullName}.",
   NotificationType.Info
);



            await _notificationService.SendToUserAsync(
                patient.UserId,
                "Appointment Scheduled",
                $"Your appointment with Dr. {doctor.FullName} has been scheduled.",
                NotificationType.Success
            );

            return Result<string>.Success("Appointment booked successfully");
        }
    }

}

