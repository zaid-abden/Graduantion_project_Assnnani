using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetTodayAppointments
{
    public class GetTodayAppointmentsQueryHandler : IRequestHandler<GetTodayAppointmentsQuery, Result<List<TodayAppointmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetTodayAppointmentsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<TodayAppointmentDto>>> Handle(GetTodayAppointmentsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<TodayAppointmentDto>>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            if (doctor is null)
                return Result<List<TodayAppointmentDto>>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var today = DateOnly.FromDateTime(DateTime.Today);
            var appointments = await unitOfWork.Appointments.Query()
                .Where(a =>
                    a.DoctorId == doctor.DoctorId &&
                    !a.IsDeleted &&
                    a.ScheduleSlot.Date == today)


                .Select(a => new TodayAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
                    Specialty = a.Doctor.Specialization.Name,
                    Time = a.ScheduleSlot.StartTime,
                    Status = a.AppointmentStatus.ToString()
                }).OrderBy(a => a.Time)
                .ToListAsync(cancellationToken);

            return Result<List<TodayAppointmentDto>>.Success(appointments);

        }
    }
}
