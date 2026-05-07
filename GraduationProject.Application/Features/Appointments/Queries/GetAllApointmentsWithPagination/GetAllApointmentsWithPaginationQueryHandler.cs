using GraduationProject.Application.Common.PaginatedResults;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAllApointmentsWithPagination
{
    public class GetAllApointmentsWithPaginationQueryHandler : IRequestHandler<GetAllApointmentsWithPaginationQuery, Result<PaginatedResult<AppointmentDtto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllApointmentsWithPaginationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedResult<AppointmentDtto>>> Handle(GetAllApointmentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var appointments = await unitOfWork.Appointments.Query()
                .Skip((request.PageNumber-1)  * request.PageSize)
                .Take(request.PageSize)
    .Include(a => a.Patient)
        .ThenInclude(p => p.User)
    .Include(a => a.Doctor)
        .ThenInclude(d => d.User)
    .Include(a => a.ScheduleSlot)
    .Select(a => new AppointmentDtto
    {
        AppointmentId = a.AppointmentId,
        StartTime = a.ScheduleSlot.StartTime,
        EndTime = a.ScheduleSlot.EndTime,
        PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
        DoctorName = a.Doctor.User.FirstName + " " + a.Doctor.User.LastName,
        Status = a.AppointmentStatus.ToString()
    })
    .ToListAsync();

            var count =await unitOfWork.Appointments.Query().CountAsync();
            return Result<PaginatedResult<AppointmentDtto>>.Success(new PaginatedResult<AppointmentDtto>(appointments,request.PageNumber,request.PageSize,count));
        }
    }
}
