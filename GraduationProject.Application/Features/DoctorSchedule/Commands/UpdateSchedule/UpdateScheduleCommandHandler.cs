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

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.UpdateSchedule
{
    public class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, Result<DoctorScheduleDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateScheduleCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<DoctorScheduleDto>> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
         
            var doctor = await unitOfWork.Doctors.GetCurrentDoctor(currentUserService.UserId);
            if (!currentUserService.IsAuthenticated)
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            }
            if (doctor == null)
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
            var schedule = await unitOfWork.DoctorSchedules.GetByIdAsync(request.ScheduleId);
            if (schedule == null)
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.NotFound, "Schedule not found.");
            }
            if (doctor.DoctorId != schedule.DoctorId)
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Unauthorized, "You are not authorized to update this schedule.");
            if (unitOfWork.DoctorSchedules.checkOverlap(doctor.DoctorId, request.DayOfWeek, request.StartTime, request.EndTime, request.Location,request.ScheduleId))
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "The schedule overlaps with an existing schedule.");
            }
            if (unitOfWork.DoctorSchedules.CheckClinicOverlap(request.DayOfWeek, request.StartTime, request.EndTime, request.Location,request.ScheduleId))
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "The schedule overlaps with another doctor's schedule at the same location.");

            }

            schedule.DayOfWeek = request.DayOfWeek;
            schedule.StartTime = request.StartTime;
            schedule.EndTime = request.EndTime;
            schedule.Location = request.Location;
            schedule.MaxAppointments = request.MaxAppointments;
            await unitOfWork.SaveAsync();
            var scheduleDto = new DoctorScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Location = schedule.Location,
                IsActive = schedule.IsActive,
                MaxAppointments = schedule.MaxAppointments
            };
            return Result<DoctorScheduleDto>.Success(scheduleDto);
        }
    }
}
