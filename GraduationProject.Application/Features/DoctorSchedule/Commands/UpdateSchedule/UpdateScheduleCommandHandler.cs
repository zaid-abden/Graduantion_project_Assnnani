using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.UpdateSchedule
{
    public class UpdateScheduleCommandHandler
    : IRequestHandler<UpdateScheduleCommand, Result<DoctorScheduleDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateScheduleCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<DoctorScheduleDto>> Handle(
            UpdateScheduleCommand request,
            CancellationToken cancellationToken)
        {
           
            if (!currentUserService.IsAuthenticated)
            {
                return Result<DoctorScheduleDto>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not authorized to perform this action.");
            }
       
            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
            {
                return Result<DoctorScheduleDto>.Failure(
                    ResultStatus.Failure,
                    "Doctor profile not found.");
            }
            
            var schedule = await unitOfWork.DoctorSchedules.GetByIdAsync(request.ScheduleId);

            if (schedule is null)
            {
                return Result<DoctorScheduleDto>.Failure(
                    ResultStatus.NotFound,
                    "Schedule not found.");
            }

            if (schedule.DoctorId != doctor.DoctorId)
            {
                return Result<DoctorScheduleDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not allowed to update this schedule.");
            }

            var doctorConflict = await unitOfWork.DoctorSchedules.Query()
      .AnyAsync(x =>
          x.DoctorId == doctor.DoctorId &&
          x.ScheduleId != schedule.ScheduleId &&
          x.IsActive &&
          x.Date == request.Date &&
          request.StartTime < x.EndTime &&
          request.EndTime > x.StartTime,
          cancellationToken);

            if (doctorConflict)
            {
                return Result<DoctorScheduleDto>.Failure(
                    ResultStatus.Conflict,
                    "This schedule overlaps with another schedule for the doctor.");
            }
          
            var clinicConflict = await unitOfWork.DoctorSchedules.Query()
                .AnyAsync(x =>
                    x.Location == request.Location &&
                    x.ScheduleId != schedule.ScheduleId &&
                    x.IsActive &&
                    x.Date == request.Date &&
                    request.StartTime < x.EndTime &&
                    request.EndTime > x.StartTime,
                    cancellationToken);

            if (clinicConflict)
            {
                return Result<DoctorScheduleDto>.Failure(
                    ResultStatus.Conflict,
                    "This time is already booked in this clinic.");
            }
         
            schedule.Date = request.Date;
            schedule.StartTime = request.StartTime;
            schedule.EndTime = request.EndTime;
            schedule.Location = request.Location;
            schedule.MaxAppointments = request.MaxAppointments;

            schedule.DayOfWeek = (WeekDay)request.Date.DayOfWeek;

            schedule.UpdatedAt = DateTime.UtcNow;
            schedule.UpdatedBy = currentUserService.UserName;
          

          
            var oldSlots = await unitOfWork.ScheduleSlots.Query()
                .Where(x => x.DoctorScheduleId == schedule.ScheduleId)
                .ToListAsync(cancellationToken);

            if (oldSlots.Any())
            {
                unitOfWork.ScheduleSlots.DeleteRange(oldSlots);
            }
          

           
            await unitOfWork.SaveAsync();



            var slots = new List<ScheduleSlot>();

            var current = TimeOnly.FromTimeSpan(schedule.StartTime);
            var endTime = TimeOnly.FromTimeSpan(schedule.EndTime);
            var slotDuration = TimeSpan.FromMinutes(30);

            while (current < endTime)
            {
                var end = current.Add(slotDuration);

                if (end > endTime)
                    break;

                slots.Add(new ScheduleSlot
                {
                    DoctorScheduleId = schedule.ScheduleId,
                    StartTime = current,
                    EndTime = end,
                    Date = schedule.Date,
                    Status = SlotStatus.Available,
                    CreatedAt = DateTime.UtcNow
                });

                current = end;
            }


            await unitOfWork.ScheduleSlots.AddRangeAsync(slots);
            await unitOfWork.SaveAsync();
       

           
            var dto = new DoctorScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Location = schedule.Location,
                IsActive = schedule.IsActive,
                MaxAppointments = schedule.MaxAppointments
            };

            return Result<DoctorScheduleDto>.Success(dto);
          
        }
    }
}
