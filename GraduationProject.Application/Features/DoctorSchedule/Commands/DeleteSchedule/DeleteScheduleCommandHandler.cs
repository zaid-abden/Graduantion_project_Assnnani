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

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeleteSchedule
{
    public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteScheduleCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
        {
            var doctor = await unitOfWork.Doctors.GetCurrentDoctor(currentUserService.UserId);
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            }
            if (doctor == null)
            {
                return Result<string>
                    .Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
           
            var schedule = await unitOfWork.DoctorSchedules.GetByIdAsync(request.ScheduleId);
            if (schedule == null)
            {
                return Result<string>.Failure(ResultStatus.NotFound, "Schedule not found.");
            }
            if(schedule.DoctorId!=doctor.DoctorId)
            {
                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to delete this schedule.");
            }
            schedule.IsActive=false;
            schedule.IsDeleted = true;
            schedule.DeletedAt = DateTime.Now;
            schedule.DeletedBy = currentUserService.UserName;
            schedule.UpdatedAt = DateTime.Now;
                schedule.UpdatedBy = currentUserService.UserName;
             
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Schedule deleted successfully.");
        }
    }
}
