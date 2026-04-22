using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.CreateSchedule
{
    public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, Result<DoctorScheduleDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
      
        public CreateScheduleCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
           
        }
        public async Task<Result<DoctorScheduleDto>> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {


            if (!currentUserService.IsAuthenticated)
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            }
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId,cancellationToken);
           
            if (doctor == null)
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
            //if (doctor.VerificationStatus != Data.Enums.DoctorVerificationStatus.Approved)
            //{
            //    return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "Doctor profile is not verified.");
            //}
            if(unitOfWork.DoctorSchedules.checkOverlap(doctor.DoctorId,request.DayOfWeek, request.StartTime, request.EndTime,request.Location))
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "The schedule overlaps with an existing schedule.");
            }
            if(unitOfWork.DoctorSchedules.CheckClinicOverlap(request.DayOfWeek,request.StartTime,request.EndTime,request.Location))
            {                 return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "The schedule overlaps with another doctor's schedule at the same location.");

            }
            var schedule = new doctorSchedule
            {
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Location = request.Location,
                MaxAppointments = request.MaxAppointments,
                DoctorId = doctor.DoctorId,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,
              
            };

            await unitOfWork.DoctorSchedules.AddAsync(schedule);



            await unitOfWork.SaveAsync();


            var slots = new List<ScheduleSlot>();

            var current = schedule.StartTime;
            var slotDuration = TimeSpan.FromMinutes(30);

            while (current < schedule.EndTime)
            {
                var end = current.Add(slotDuration);

                if (end > schedule.EndTime)
                    break;

                
                var exists = await unitOfWork.ScheduleSlots.Query()
                    .AnyAsync(s =>
                        s.DoctorScheduleId == schedule.ScheduleId &&
                        current < s.EndTime &&
                        end > s.StartTime,
                        cancellationToken);

                if (exists)
                {
                    return Result<DoctorScheduleDto>.Failure(
                        ResultStatus.Failure,
                        $"Slot overlap detected at {current}");
                }

                slots.Add(new ScheduleSlot
                {
                    DoctorScheduleId = schedule.ScheduleId,
                    StartTime = current,
                    EndTime = end,
                    Status = SlotStatus.Available,
                    CreatedAt = DateTime.Now
                });

                current = end;
            }





            await unitOfWork.ScheduleSlots.AddRangeAsync(slots);

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
