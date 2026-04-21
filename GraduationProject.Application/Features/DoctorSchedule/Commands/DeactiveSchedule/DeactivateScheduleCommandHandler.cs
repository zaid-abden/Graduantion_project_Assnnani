using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeactiveSchedule
{
    public class DeactivateScheduleCommandHandler : IRequestHandler<DeactivateScheduleCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeactivateScheduleCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(DeactivateScheduleCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not authorized to perform this action.");
            }

            var userId = currentUserService.UserId;

            var schedule = await unitOfWork.DoctorSchedules.Query()
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x =>
                    x.ScheduleId == request.ScheduleId &&
                    x.Doctor.UserId == userId,
                    cancellationToken);

            if (schedule is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Schedule not found or you are not authorized to access it.");
            }

            if (!schedule.IsActive)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Schedule is already inactive.");
            }

            var hasAppointments = await unitOfWork.Appointments.Query()
                .AnyAsync(c =>
                    c.DoctorScheduleId == schedule.ScheduleId &&
                    !c.IsDeleted,
                    cancellationToken);

            if (hasAppointments)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Cannot deactivate schedule because it has active booked appointments.");
            }

            schedule.IsActive = false;
            schedule.UpdatedAt = DateTime.Now;
            schedule.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Schedule deactivated successfully.");
        }
    }
}
