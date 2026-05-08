using GraduationProject.Application.Common.PaginatedResults;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatients
{
    //public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, Result<PaginatedResult<PatientForDoctorDashboardDto>>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public GetPatientsQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }
    //    public async Task<Result<PaginatedResult<PatientForDoctorDashboardDto>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
    //    {

    //        if (!currentUserService.IsAuthenticated)
    //            return Result<PaginatedResult<PatientForDoctorDashboardDto>>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
    //        var userId = currentUserService.UserId;
    //        var doctor = await unitOfWork.Doctors.Query()
    //            .FirstOrDefaultAsync(x => x.UserId == userId
    //            , cancellationToken);
    //        if (doctor is null)
    //            return Result<PaginatedResult<PatientForDoctorDashboardDto>>.Failure(ResultStatus.NotFound, "Doctor profile not found");

    //        var query = unitOfWork.Patients.Query()
    //  .AsNoTracking()
    //  .Include(x => x.User)
    //  .Include(x => x.AssignedDoctor)
    //  .ThenInclude(d => d.User)
    //  .Include(x => x.Appointments)
    //  .ThenInclude(a => a.ScheduleSlot)
    //  .Where(x =>
    //      x.AssignedDoctorId == doctor.DoctorId ||
    //      x.Appointments.Any(a => a.DoctorId == doctor.DoctorId));

    //        if (!string.IsNullOrWhiteSpace(request.Search))
    //        {
    //            query = query.Where(x =>
    //                x.User.FullName.Contains(request.Search) ||
    //                (x.User.PhoneNumber != null && x.User.PhoneNumber.Contains(request.Search)));
    //        }

    //        if (request.PatientStatus.HasValue)
    //        {
    //            query = query.Where(x => x.Status == (PatientStatus)request.PatientStatus.Value);
    //        }
    //        var count = await query.CountAsync(cancellationToken);
    //        var patients = await query
    //            .Skip((request.PageNumber - 1) * request.PageSize)
    //            .Take(request.PageSize)
    //            .Select(x => new PatientForDoctorDashboardDto
    //            {
    //                Id = x.PatientId,
    //                Name = x.User.FullName,
    //                Gender = x.Gender.ToString(),
    //                Age = DateTime.Today.Year - x.DateOfBirth.Year -
    //  (DateTime.Today.DayOfYear < x.DateOfBirth.DayOfYear ? 1 : 0),

    //                LastVisit = x.Appointments
    //.OrderByDescending(a => a.ScheduleSlot.Date)
    //.Select(a => a.ScheduleSlot.Date)
    //.FirstOrDefault(),
    //                AssignedDoctor = x.AssignedDoctor!.User.FullName,
    //                Phone = x.User.PhoneNumber!,
    //                Status = x.Status.ToString()
    //            }).ToListAsync();

    //        return Result<PaginatedResult<PatientForDoctorDashboardDto>>
    //            .Success(new PaginatedResult<PatientForDoctorDashboardDto>
    //            (patients, request.PageNumber, request.PageSize, count));
    //    }
    //}
    public class GetPatientsQueryHandler : IRequestHandler<GetPatientsQuery, Result<PaginatedResult<PatientForDoctorDashboardDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPatientsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<PatientForDoctorDashboardDto>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
        {
           
            if (!currentUserService.IsAuthenticated)
                return Result<PaginatedResult<PatientForDoctorDashboardDto>>
                    .Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
                return Result<PaginatedResult<PatientForDoctorDashboardDto>>
                    .Failure(ResultStatus.NotFound, "Doctor profile not found");

           
            var query = unitOfWork.Patients.Query()
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.AssignedDoctor)
                    .ThenInclude(d => d.User)
                .Include(x => x.Appointments)
                    .ThenInclude(a => a.ScheduleSlot)
                .Where(x =>
                    x.AssignedDoctorId == doctor.DoctorId ||
                    x.Appointments.Any(a => a.DoctorId == doctor.DoctorId));

           
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.User.FullName.Contains(request.Search) ||
                    (x.User.PhoneNumber != null && x.User.PhoneNumber.Contains(request.Search)));
            }

          
            if (request.PatientStatus.HasValue)
            {
                query = query.Where(x => x.Status == (PatientStatus)request.PatientStatus.Value);
            }

            var count = await query.CountAsync(cancellationToken);

            var patients = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PatientForDoctorDashboardDto
                {
                    Id = x.PatientId,
                    Name = x.User.FullName,
                    Gender = x.Gender.ToString(),

                    Age = DateTime.Today.Year - x.DateOfBirth.Year -
                          (DateTime.Today.DayOfYear < x.DateOfBirth.DayOfYear ? 1 : 0),


                    LastVisit = x.Appointments
    .Where(a => a.AppointmentStatus == AppointmentStatus.Completed)
    .OrderByDescending(a => a.ScheduleSlot.Date)
    .Select(a => (DateOnly?)a.ScheduleSlot.Date)
    .FirstOrDefault(),
                   
                    Phone = x.User.PhoneNumber!,
                    Status = x.Status.ToString()
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<PatientForDoctorDashboardDto>>
                .Success(new PaginatedResult<PatientForDoctorDashboardDto>(
                    patients,
                    request.PageNumber,
                    request.PageSize,
                    count
                ));
        }
    }
}
