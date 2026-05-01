//using GraduationProject.Application.Common.Results;
//using GraduationProject.Application.Contracts.Identity;
//using GraduationProject.Application.Contracts.Repositories;
//using GraduationProject.Data.Enums;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraduationProject.Application.Features.DoctorSchedule.Commands.RestoreSchedule
//{
//    public class RestoreScheduleCommandHandler : IRequestHandler<RestoreScheduleCommand, Result<string>>
//    {
//        private readonly IUnitOfWork unitOfWork;
//        private readonly ICurrentUserService currentUserService;

//        public RestoreScheduleCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
//        {
//            this.unitOfWork = unitOfWork;
//            this.currentUserService = currentUserService;
//        }
//        public async Task<Result<string>> Handle(RestoreScheduleCommand request, CancellationToken cancellationToken)
//        {
//            if (!currentUserService.IsAuthenticated)
//            {
//                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
//            }

//            var userId = currentUserService.UserId;

//            var doctor = await unitOfWork.Doctors.Query()
//                .Include(x => x.User)
//                .FirstOrDefaultAsync(x => x.UserId == userId
              
//                , cancellationToken);

//            if (doctor is null)
//                return Result<string>.Failure(ResultStatus.Failure, "Doctor profile not found.");

//            var schedule = await unitOfWork.DoctorSchedules.Query()
//                .FirstOrDefaultAsync(x => x.ScheduleId == request.ScheduleId
//                && x.IsDeleted, cancellationToken);

//            if (schedule is null)
//                return Result<string>.Failure(ResultStatus.NotFound, "Schedule not found.");

//            if (doctor.DoctorId != schedule.DoctorId)
//                return Result<string>.Failure(ResultStatus.Forbidden, "You are not allowed to update schedules that are not assigned to your account.");
//            var hasConflict = await unitOfWork.DoctorSchedules.Query()
//     .AnyAsync(x =>
//         x.DoctorId == schedule.DoctorId &&
//         x.ScheduleId != schedule.ScheduleId &&
//         x.IsActive &&
//         x.DayOfWeek == schedule.DayOfWeek &&
//         schedule.StartTime < x.EndTime &&
//         schedule.EndTime > x.StartTime,
//         cancellationToken);

//            if (hasConflict)
//            {
//                return Result<string>.Failure(
//                    ResultStatus.Conflict,
//                    "Cannot restore schedule because it conflicts with another active schedule.");
//            }
//            schedule.IsDeleted = false;
//            schedule.DeletedAt = null;
//            schedule.DeletedBy = null;
//            schedule.UpdatedAt = DateTime.Now;
//            schedule.UpdatedBy = currentUserService.UserName;
//    //        await unitOfWork.ScheduleSlots.Query()
//    //.Where(x => x.DoctorScheduleId == schedule.ScheduleId
//    //         && x.Status == SlotStatus.Cancelled)
//    //.ExecuteUpdateAsync(s =>
//    //    s.SetProperty(x => x.Status, SlotStatus.Available),
//    //    cancellationToken);

//            await unitOfWork.SaveAsync();
//            return Result<string>.Success("Schedule restored successfully.");
//        }
//    }
//}
