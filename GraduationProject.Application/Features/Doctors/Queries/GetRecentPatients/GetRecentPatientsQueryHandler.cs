using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetRecentPatients
{
    public class GetRecentPatientsQueryHandler : IRequestHandler<GetRecentPatientsQuery, Result<List<RecentPatientDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetRecentPatientsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<RecentPatientDto>>> Handle(GetRecentPatientsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<RecentPatientDto>>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<List<RecentPatientDto>>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var recentPatients = await unitOfWork.Appointments.Query()
     .AsNoTracking()
     .Where(c => !c.IsDeleted &&
                 c.DoctorId == doctor.DoctorId &&
                 c.AppointmentStatus != AppointmentStatus.Cancelled)
     .Select(x => new
     {
         x.PatientId,
         Name = x.Patient.User.FullName,
         ImageUrl = x.Patient.User.ImageUrl,
         Date = x.ScheduleSlot.Date
     })
     .GroupBy(x => x.PatientId)
     .Select(g => new RecentPatientDto
     {
         Id = g.Key,
         Name = g.First().Name,
         ImageUrl = g.First().ImageUrl,
         LastInteractionDate = g.Max(x => x.Date)
     })
     .OrderByDescending(x => x.LastInteractionDate)
     .Take(request.Count)
     .ToListAsync(cancellationToken);

            return Result<List<RecentPatientDto>>.Success(recentPatients);
        }
    }
}
