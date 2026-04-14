using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using GraduationProject.Application.Features.Specializations.Dtos;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetDoctorsBySpecialization
{
    public class GetDoctorsBySpecializationQueryHandler : IRequestHandler<GetDoctorsBySpecializationQuery, Result<List<DoctorForPatientDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> UserManager;
        public GetDoctorsBySpecializationQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            UserManager = userManager;
        }



        public async Task<Result<List<DoctorForPatientDto>>> Handle(GetDoctorsBySpecializationQuery request, CancellationToken cancellationToken)
        {
            var specialization = await unitOfWork.Specialization.GetByIdAsync(request.Id);
            if (specialization == null)
            {
                return Result<List<DoctorForPatientDto>>.Failure(
                    ResultStatus.NotFound,
                    "Specialization not found."
                );
            }

            var doctors = await unitOfWork.Doctors
     .GetAll()
     .Where(x => x.SpecializationId == request.Id)
     .Include(x => x.Schedules)
     .Select(x => new DoctorForPatientDto
     {
         FullName = x.FullName!,
         YearsOfExperience = x.YearsOfExperience,
         Rating = x.Rating,
         RatingCount = x.RatingCount,
         SpecializationName = specialization.Name,
         Schedules = x.Schedules.Select(s => new DoctorScheduleDtoForPatient
         {
             DayOfWeek = s.DayOfWeek,
             StartTime = s.StartTime,
             EndTime = s.EndTime,
             Location = s.Location,

         }).ToList()
     })
     .ToListAsync();

            if (!doctors.Any())
                return Result<List<DoctorForPatientDto>>.Failure(ResultStatus.NotFound, "No doctors found for this specialization.");

            return Result<List<DoctorForPatientDto>>.Success(doctors);
        }
    }
}
