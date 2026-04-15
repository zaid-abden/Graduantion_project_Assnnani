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
            var doctor = await unitOfWork.Doctors.GetCurrentDoctor(currentUserService.UserId);
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            }
            if (doctor == null)
            {
                return Result<string>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
            var schedule = await unitOfWork.DoctorSchedules.GetByIdAsync(request.ScheduleId);
            if (schedule == null)
            {
                return Result<string>.Failure(ResultStatus.NotFound, "Schedule not found.");
            }
            if (doctor.DoctorId != schedule.DoctorId)
                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to update this schedule.");
            if(schedule.IsActive==true)
                return Result<string>.Failure(ResultStatus.Failure, "Schedule is already active.");
            schedule.IsActive = true;
            await unitOfWork.SaveAsync();

            return Result<string>.Success("Schedule activated successfully.");
        }
    }
}
