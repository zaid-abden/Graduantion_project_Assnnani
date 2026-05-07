using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetAppointmentDetails
{
    //public class GetAppointmentDetailsQueryHandler : IRequestHandler<GetAppointmentDetailsQuery, Result<AppointmentDetailsDto>>
    //{
    //    public Task<Result<AppointmentDetailsDto>> Handle(GetAppointmentDetailsQuery request, CancellationToken cancellationToken)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public class GetAppointmentDetailsQueryHandler
    : IRequestHandler<GetAppointmentDetailsQuery, Result<AppointmentDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetAppointmentDetailsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<AppointmentDetailsDto>> Handle(
            GetAppointmentDetailsQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
                return Result<AppointmentDetailsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized");

            var userId = _currentUserService.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<AppointmentDetailsDto>.Failure(ResultStatus.Forbidden, "You are not authorized to perform this action.");

            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.Patient)
                    .ThenInclude(p => p.User)
                .Include(x => x.Doctor)
                .Include(x => x.ScheduleSlot)
                .ThenInclude(x => x.DoctorSchedule)
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(x =>
                    x.AppointmentId == request.Id &&
                    x.DoctorId == receptionist.DoctorId,   
                    cancellationToken);

            if (appointment is null)
                return Result<AppointmentDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Appointment not found");

            var dto = new AppointmentDetailsDto
            {
                Id = appointment.AppointmentId,
                Status = appointment.AppointmentStatus.ToString(),

                Date = appointment.ScheduleSlot.Date,
                Time = appointment.ScheduleSlot.StartTime,

                Location = appointment.ScheduleSlot.DoctorSchedule.Doctor.ClinicLocation!,

                Duration = (int)(
    appointment.ScheduleSlot.EndTime.ToTimeSpan() -
    appointment.ScheduleSlot.StartTime.ToTimeSpan()
).TotalMinutes,

                Type = appointment.AppointmentType.ToString(),

                Notes = appointment.Notes ?? "",

                PatientName = appointment.Patient.User.FirstName + " " + appointment.Patient.User.LastName,
                DoctorName = appointment.Doctor.FullName!,

                Mode = appointment.BookingType,
                PaymentStatus = appointment.PaymentStatus
            };

            return Result<AppointmentDetailsDto>.Success(dto);
        }
    }
}
