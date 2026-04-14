using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetSchedulesByDoctor_Day
{
    public class GetSchedulesByDoctorDayQueryHandler : IRequestHandler<GetSchedulesByDoctorDayQuery, Result<List<DoctorScheduleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetSchedulesByDoctorDayQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<DoctorScheduleDto>>> Handle(GetSchedulesByDoctorDayQuery request, CancellationToken cancellationToken)
        {
            var schedules = (await unitOfWork.DoctorSchedules.GetAllAsync())
                .Where(x => x.DoctorId == request.DoctorId && x.DayOfWeek == request.Day);
            var schedulesDto = schedules.Select(x => new DoctorScheduleDto
            {
                ScheduleId = x.ScheduleId,
                DayOfWeek = x.DayOfWeek,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Location = x.Location,
                IsActive = x.IsActive,
                MaxAppointments = x.MaxAppointments
            }).ToList();

            return Result<List<DoctorScheduleDto>>.Success(schedulesDto);
        }
    }
}
