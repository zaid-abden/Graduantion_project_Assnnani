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

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllActiveScheduleForDoctor
{
    public class GetAllActiveScheduleForDoctorQueryHandler : IRequestHandler<GetAllActiveScheduleForDoctorQuery, Result<List<DoctorScheduleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllActiveScheduleForDoctorQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<DoctorScheduleDto>>> Handle(GetAllActiveScheduleForDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctor = await unitOfWork.Doctors.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                return Result<List<DoctorScheduleDto>>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
            var schedules = (await unitOfWork.DoctorSchedules.GetAllAsync())
                .Where(c => c.DoctorId == request.DoctorId && c.IsActive);
            var scheduleDtos = schedules.Select(schedule => new DoctorScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Location = schedule.Location,
                IsActive = schedule.IsActive,
                MaxAppointments = schedule.MaxAppointments
            }).ToList();

                 return Result<List<DoctorScheduleDto>>.Success(scheduleDtos);
        }
    }
}
