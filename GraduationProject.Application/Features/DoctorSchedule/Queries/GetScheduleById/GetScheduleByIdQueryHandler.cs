//using GraduationProject.Application.Common.Results;
//using GraduationProject.Application.Contracts.Identity;
//using GraduationProject.Application.Contracts.Repositories;
//using GraduationProject.Application.Features.DoctorSchedule.Dtos;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetScheduleById
//{
//    public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, Result<DoctorScheduleDto>>
//    {
//        private readonly IUnitOfWork unitOfWork;
//        private readonly ICurrentUserService currentUserService;

//        public GetScheduleByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
//        {
//            this.unitOfWork = unitOfWork;
//            this.currentUserService = currentUserService;
//        }
//        public async Task<Result<DoctorScheduleDto>> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
//        {
//            if (!currentUserService.IsAuthenticated)
//            {
//                return Result<DoctorScheduleDto>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
//            }
//            var userId = currentUserService.UserId;
//            var doctor = await unitOfWork.Doctors.Query()
//                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

//            if (doctor == null)
//            {
//                return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "Doctor profile not found.");
//            }
//            var schedule = await unitOfWork.DoctorSchedules.GetByIdAsync(request.ScheduleId);
//            if (schedule == null || schedule.DoctorId != doctor.DoctorId)
//            {
//                return Result<DoctorScheduleDto>.Failure(ResultStatus.NotFound, "Schedule not found.");
//            }
//            var scheduleDto = new DoctorScheduleDto
//            {
//                ScheduleId = schedule.ScheduleId,
//                DayOfWeek = schedule.DayOfWeek,
//                StartTime = schedule.StartTime,
//                EndTime = schedule.EndTime,
//                Location = schedule.Location,
//                IsActive = schedule.IsActive,
//                MaxAppointments = schedule.MaxAppointments
//            };
//            return Result<DoctorScheduleDto>.Success(scheduleDto);
//        }
//    }
//}
