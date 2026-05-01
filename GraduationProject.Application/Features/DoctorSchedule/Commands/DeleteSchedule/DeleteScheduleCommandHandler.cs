//using GraduationProject.Application.Common.Results;
//using GraduationProject.Application.Contracts.Identity;
//using GraduationProject.Application.Contracts.Repositories;
//using GraduationProject.Application.Features.DoctorSchedule.Dtos;
//using GraduationProject.Data.Enums;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeleteSchedule
//{
//    public class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleCommand, Result<string>>
//    {
//        private readonly IUnitOfWork unitOfWork;
//        private readonly ICurrentUserService currentUserService;

//        public DeleteScheduleCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
//        {
//            this.unitOfWork = unitOfWork;
//            this.currentUserService = currentUserService;
//        }
//        public async Task<Result<string>> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
//        {
//            if (!currentUserService.IsAuthenticated)
//            {
//                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
//            }
//            var userId = currentUserService.UserId;
//            var doctor = await unitOfWork.Doctors.Query()
//                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

//            if (doctor == null)
//            {
//                return Result<string>
//                    .Failure(ResultStatus.Failure, "Doctor profile not found.");
//            }
           
//            var schedule = await unitOfWork.DoctorSchedules.Query()
//                .FirstOrDefaultAsync(x => x.ScheduleId == request.ScheduleId
//                && !x.IsDeleted,cancellationToken);
//            if (schedule == null)
//            {
//                return Result<string>.Failure(ResultStatus.NotFound, "Schedule not found.");
//            }
//            if(schedule.DoctorId!=doctor.DoctorId)
//            {
//                return Result<string>.Failure(ResultStatus.Unauthorized, "You are not authorized to delete this schedule.");
//            }

//            var hasAppointments = await unitOfWork.Appointments.Query()
//     .AnyAsync(x => x.ScheduleSlot.DoctorScheduleId == schedule.ScheduleId,
//         cancellationToken);

//            if (hasAppointments)
//                return Result<string>.Failure(
//                    ResultStatus.Conflict,
//                    "Cannot delete this schedule because it already has booked appointments."
//                );


//            schedule.IsActive=false;
//            schedule.IsDeleted = true;
//            schedule.DeletedAt = DateTime.Now;
//            schedule.DeletedBy = currentUserService.UserName;
//            schedule.UpdatedAt = DateTime.Now;
//                schedule.UpdatedBy = currentUserService.UserName;

//    //        var cancelledSlotsCount = await unitOfWork.ScheduleSlots.Query()
//    //.Where(x => x.DoctorScheduleId == schedule.ScheduleId 
//    //&& x.Status == SlotStatus.Available)
//    //.ExecuteUpdateAsync(
//    //    setters => setters.SetProperty(x => x.Status, SlotStatus.Cancelled),
//    //    cancellationToken
//    //);
//            await unitOfWork.SaveAsync();
//            return Result<string>.Success("Schedule has been deactivated successfully");
//        }
//    }
//}
