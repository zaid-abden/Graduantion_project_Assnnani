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

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeleteScheduleSlot
{
    public class DeleteScheduleSlotCommandHandler : IRequestHandler<DeleteScheduleSlotCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteScheduleSlotCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(DeleteScheduleSlotCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");
            var slot = await unitOfWork.ScheduleSlots.Query()
                .FirstOrDefaultAsync(x => x.Id == request.SlotId
                && x.DoctorSchedule.DoctorId == doctor.DoctorId
                , cancellationToken);
            if(slot is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Schedule slot not found");
            if(slot.Status == SlotStatus.Booked)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot delete a booked slot. Please cancel the appointment first.");
            if(slot.Status == SlotStatus.Blocked)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot delete a blocked slot. Please unblock it first.");
            slot.Status = SlotStatus.Blocked;
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Schedule slot deleted successfully.");
        }
    }
}
