using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllSchedules
{
    public class GetAllSchedulesQueryHandler : IRequestHandler<GetAllSchedulesQuery, Result<List<DoctorScheduleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllSchedulesQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<DoctorScheduleDto>>> Handle(GetAllSchedulesQuery request, CancellationToken cancellationToken)
        {
            var doctor = await unitOfWork.Doctors.GetCurrentDoctor(currentUserService.UserId);
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<DoctorScheduleDto>>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            }
            if (doctor == null)
            {
                return Result<List<DoctorScheduleDto>>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
         
            var doctorSchedules = (await unitOfWork.DoctorSchedules.GetAllAsync())
                 .OrderBy(s => s.DayOfWeek)
                 .ThenBy(s => s.StartTime)
                      .Where(s => s.DoctorId == doctor.DoctorId)
                      .ToList();
            var scheduleDtos = doctorSchedules
      .Select(s => new DoctorScheduleDto
      {
          ScheduleId=s.ScheduleId,
          DayOfWeek = s.DayOfWeek,
          StartTime = s.StartTime,
          EndTime = s.EndTime,
          Location = s.Location,
          IsActive = s.IsActive,
          MaxAppointments = s.MaxAppointments,
      })
      .ToList();
           
            return Result<List<DoctorScheduleDto>>.Success(scheduleDtos);


        }
    }
}
