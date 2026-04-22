using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GellAllActiveSchedule
{
    public class GetAllActiveScheduleQueryHandler : IRequestHandler<GetAllActiveScheduleQuery, Result<List<DoctorScheduleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllActiveScheduleQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<DoctorScheduleDto>>> Handle(GetAllActiveScheduleQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<DoctorScheduleDto>>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            }
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor == null)
            {
                return Result<List<DoctorScheduleDto>>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
            var shedule=(await unitOfWork.DoctorSchedules.GetAllAsync())
                .Where(s => s.DoctorId == doctor.DoctorId && s.IsActive).ToList();
            var scheduleDtos = shedule
     .Select(schedule => new DoctorScheduleDto
     {
         ScheduleId = schedule.ScheduleId,
         DayOfWeek = schedule.DayOfWeek,
         StartTime = schedule.StartTime,
         EndTime = schedule.EndTime,
         Location = schedule.Location,
         IsActive = schedule.IsActive,
         MaxAppointments = schedule.MaxAppointments
     })
     .ToList();
            return Result<List<DoctorScheduleDto>>.Success(scheduleDtos);
        }
    }
}
