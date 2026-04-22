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

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.MakeAScheduleActive
{
    public class MakeAScheduleActiveCommandHandler : IRequestHandler<MakeAScheduleActiveCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public MakeAScheduleActiveCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(MakeAScheduleActiveCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            }

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
                return Result<string>.Failure(ResultStatus.Failure, "Doctor profile not found.");

            var schedule = await unitOfWork.DoctorSchedules.Query()
                .FirstOrDefaultAsync(x => x.ScheduleId == request.ScheduleId
                && !x.IsDeleted, cancellationToken);

            if (schedule is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Schedule not found.");

            if (doctor.DoctorId != schedule.DoctorId)
                return Result<string>.Failure(ResultStatus.Forbidden, "You are not allowed to update schedules that are not assigned to your account.");

           
            if (schedule.IsActive)
                return Result<string>.Failure(ResultStatus.Conflict, "Schedule is already active.");

            var hasConflict = await unitOfWork.DoctorSchedules.Query()
                .AnyAsync(x =>
                    x.DoctorId == schedule.DoctorId &&
                    x.ScheduleId != request.ScheduleId &&
                    x.IsActive &&
                    x.DayOfWeek == schedule.DayOfWeek &&
                    schedule.StartTime < x.EndTime &&
                    schedule.EndTime > x.StartTime,
                    cancellationToken);

            if (hasConflict)
                return Result<string>.Failure(ResultStatus.Conflict, "Schedule conflicts with another active schedule.");

            schedule.IsActive = true;
            schedule.UpdatedAt = DateTime.Now;
            schedule.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Schedule activated successfully.");
        }
    }
}
