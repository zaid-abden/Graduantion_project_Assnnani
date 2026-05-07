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

namespace GraduationProject.Application.Features.Receptionists.Commands.CheckInAppointment
{
    //public class CheckInAppointmentCommandHandler
    //   : IRequestHandler<CheckInAppointmentCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;

    //    public CheckInAppointmentCommandHandler(IUnitOfWork unitOfWork)
    //    {
    //        _unitOfWork = unitOfWork;
    //    }

    //    public async Task<Result<string>> Handle(
    //        CheckInAppointmentCommand request,
    //        CancellationToken cancellationToken)
    //    {
    //        var appointment = await _unitOfWork.Appointments.Query()
    //            .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId,
    //                cancellationToken);

    //        if (appointment is null)
    //            return Result<string>.Failure(ResultStatus.NotFound, "Appointment not found");


    //        if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Cannot check-in cancelled appointment");

    //        if (appointment.AppointmentStatus == AppointmentStatus.Completed)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Appointment already completed");

    //        if (appointment.AppointmentStatus != AppointmentStatus.Confirmed)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Only confirmed appointments can check-in");


    //        appointment.IsCheckedIn = true;
    //        appointment.ArrivedAt = TimeOnly.FromDateTime(DateTime.Now);
    //        appointment.QueueStatus = QueueStatus.Waiting;
    //        appointment.ArrivelTime = TimeOnly.FromDateTime(DateTime.Now);
    //        await _unitOfWork.SaveAsync();

    //        return Result<string>.Success("Patient checked in successfully");
    //    }
    //}
    //public class CheckInAppointmentCommandHandler
    //: IRequestHandler<CheckInAppointmentCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly INotificationService _notificationService;

    //    public CheckInAppointmentCommandHandler(IUnitOfWork unitOfWork,INotificationService notificationService)
    //    {
    //        _unitOfWork = unitOfWork;
    //        this._notificationService = notificationService;
    //    }

    //    public async Task<Result<string>> Handle(
    //        CheckInAppointmentCommand request,
    //        CancellationToken cancellationToken)
    //    {
    //        var appointment = await _unitOfWork.Appointments.Query()
    //            .Include(x => x.Patient)
    //            .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId,
    //                cancellationToken);

    //        if (appointment is null)
    //            return Result<string>.Failure(ResultStatus.NotFound, "Appointment not found");


    //        if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Cannot check-in cancelled appointment");

    //        if (appointment.AppointmentStatus == AppointmentStatus.Completed)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Appointment already completed");

    //        if (appointment.AppointmentStatus != AppointmentStatus.Confirmed)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Only confirmed appointments can check-in");


    //        if (appointment.IsCheckedIn)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Patient already checked in");

    //        var patientId = await _unitOfWork.Appointments.Query()
    //            .Include(x => x.Patient)
    //            .Where(a => a.AppointmentId == request.AppointmentId
    //                ).Select(x => x.PatientId).FirstOrDefaultAsync(cancellationToken);

    //        var patient = await _unitOfWork.Patients.Query()
    //            .FirstOrDefaultAsync(x => x.PatientId == patientId, cancellationToken);
    //        if (patient is null)
    //            return Result<string>.Failure(ResultStatus.NotFound, "Patient not found");
    //        if (patient.Status != PatientStatus.Active)
    //        {
    //            patient.Status = PatientStatus.Active;
    //        }



    //        appointment.IsCheckedIn = true;
    //        appointment.ArrivedAt = TimeOnly.FromDateTime(DateTime.Now);
    //        appointment.ArrivelTime = TimeOnly.FromDateTime(DateTime.Now);


    //        appointment.QueueStatus = QueueStatus.Arrived;



    //        await _unitOfWork.SaveAsync();

    //        return Result<string>.Success("Patient checked in successfully");
    //    }
    public class CheckInAppointmentCommandHandler
    : IRequestHandler<CheckInAppointmentCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly ICurrentUserService _currentUser;

        public CheckInAppointmentCommandHandler(
            IUnitOfWork unitOfWork,
            INotificationService notificationService,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _currentUser = currentUser;
        }

        public async Task<Result<string>> Handle(
            CheckInAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Access denied. Please login to continue."
                );

            var receptionistId = _currentUser.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == receptionistId,
                    cancellationToken);

            if (receptionist is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Receptionist account not found."
                );

            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.Patient)
                .ThenInclude(c => c.User)
                .Include(x => x.Doctor)
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
                    ResultStatus.Forbidden,
                    "You are not authorized to manage this appointment."
                );

            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Cannot check in a cancelled appointment."
                );

            if (appointment.AppointmentStatus == AppointmentStatus.Completed)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "This appointment has already been completed."
                );

            if (appointment.AppointmentStatus != AppointmentStatus.Confirmed)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Only confirmed appointments can be checked in."
                );

            if (appointment.IsCheckedIn)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Patient has already been checked in."
                );

            var patient = await _unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(
                    x => x.PatientId == appointment.PatientId,
                    cancellationToken);

            if (patient is null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Patient not found."
                );

            if (patient.Status != PatientStatus.Active)
            {
                patient.Status = PatientStatus.Active;
            }

            appointment.IsCheckedIn = true;
            appointment.ArrivedAt = TimeOnly.FromDateTime(DateTime.Now);
            appointment.ArrivelTime = TimeOnly.FromDateTime(DateTime.Now);
            appointment.QueueStatus = QueueStatus.Arrived;


         

            await _unitOfWork.SaveAsync();
            await _notificationService.SendToUserAsync(
 appointment.Doctor.UserId,
 "Patient Checked In",
 $"Patient {appointment.Patient.User.FullName} has checked in.",
 NotificationType.Info
);


            await _notificationService.SendToUserAsync(
                appointment.Patient.UserId,
                "Check-in Successful",
                "You have successfully checked in for your appointment.",
                NotificationType.Success
            );

            return Result<string>.Success(
                "Patient checked in successfully."
            );
        }
    }
}

