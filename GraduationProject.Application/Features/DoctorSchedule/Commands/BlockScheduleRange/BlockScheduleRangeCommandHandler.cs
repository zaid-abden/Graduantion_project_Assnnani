using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.BlockScheduleRange
{
    public class BlockScheduleRangeCommandHandler : IRequestHandler<BlockScheduleRangeCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BlockScheduleRangeCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BlockScheduleRangeCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var doctorShedule = await unitOfWork.DoctorSchedules.Query()
    .FirstOrDefaultAsync(x => x.ScheduleId == request.Id && x.IsActive, cancellationToken);

            if (doctorShedule is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Active doctor schedule not found");

            if (doctorShedule.DoctorId != doctor.DoctorId)
                return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");

          
            var affectedRows = await unitOfWork.ScheduleSlots.Query()
                .Where(x => x.DoctorScheduleId == request.Id
                    && x.StartTime >= request.Start
                    && x.EndTime <= request.End
                    && x.Status != SlotStatus.Booked)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(s => s.Status, SlotStatus.Blocked),
                    cancellationToken);

           
            if (affectedRows == 0)
                return Result<string>.Failure(ResultStatus.NotFound, "No available slots found in this range");

            return Result<string>.Success($"{affectedRows} slots blocked successfully.");
        }
    }
}
