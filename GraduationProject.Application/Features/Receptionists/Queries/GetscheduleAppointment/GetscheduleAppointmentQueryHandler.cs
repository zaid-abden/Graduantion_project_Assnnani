using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Receptionists.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetscheduleAppointment
{
    public class GetRescheduleAppointmentQueryHandler
     : IRequestHandler<GetscheduleAppointmentQuery, Result<scheduleAppointmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetRescheduleAppointmentQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<scheduleAppointmentDto>> Handle(
            GetscheduleAppointmentQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<scheduleAppointmentDto>.Failure(ResultStatus.NotFound, "Receptionist not found");

            var appointment = await _unitOfWork.Appointments.Query()
                .Include(x => x.Patient).ThenInclude(p => p.User)
                .Include(x => x.Doctor)
                .Include(x => x.ScheduleSlot)
                .FirstOrDefaultAsync(x =>
                    x.AppointmentId == request.AppointmentId &&
                    x.DoctorId == receptionist.DoctorId,
                    cancellationToken);

            if (appointment is null)
                return Result<scheduleAppointmentDto>.Failure(ResultStatus.NotFound, "Appointment not found");

            var dto = new scheduleAppointmentDto
            {
                PatientName = appointment.Patient.User.FirstName + " " + appointment.Patient.User.LastName,
                DoctorName = appointment.Doctor.FullName!,
                CurrentDate = appointment.ScheduleSlot.Date.ToString(),
                CurrentTime = appointment.ScheduleSlot.StartTime.ToString()
            };

            return Result<scheduleAppointmentDto>.Success(dto);
        }
    }
}
