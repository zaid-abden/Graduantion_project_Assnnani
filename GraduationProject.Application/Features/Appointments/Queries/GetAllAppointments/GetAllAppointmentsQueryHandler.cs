using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAllAppointments
{
    public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, Result<List<AppointmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllAppointmentsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<AppointmentDto>>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var appointments = await unitOfWork.Appointments.Query()
    .Include(a => a.Patient)
        .ThenInclude(p => p.User)
    .Include(a => a.Doctor)
        .ThenInclude(d => d.User)
    .Include(a => a.ScheduleSlot)
    .Select(a => new AppointmentDto
    {
        AppointmentId = a.AppointmentId,
        StartTime = a.ScheduleSlot.StartTime,
        EndTime = a.ScheduleSlot.EndTime,
        PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
        DoctorName = a.Doctor.User.FirstName + " " + a.Doctor.User.LastName,
        Status = a.AppointmentStatus.ToString()
    })
    .ToListAsync();

            return Result<List<AppointmentDto>>.Success(appointments);
        }
    }
}
