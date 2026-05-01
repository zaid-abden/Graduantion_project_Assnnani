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

        public CreateScheduleCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
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
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor == null)
            {
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            }
            //if (doctor.VerificationStatus != Data.Enums.DoctorVerificationStatus.Approved)
            //{
            //    return Result<DoctorScheduleDto>.Failure(ResultStatus.Failure, "Doctor profile is not verified.");
            //}
            var doctorConflict = await unitOfWork.DoctorSchedules.Query()
        .AnyAsync(x =>
            x.DoctorId == doctor.DoctorId &&
            x.Date == request.Date &&
            x.IsActive &&
            request.StartTime < x.EndTime &&
            request.EndTime > x.StartTime,
            cancellationToken);

            if (doctorConflict)
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Conflict,
                    "Doctor already has a conflicting schedule.");

            var clinicConflict = await unitOfWork.DoctorSchedules.Query()
                .AnyAsync(x =>
                    x.Location == request.Location &&
                    x.Date == request.Date &&
                    x.IsActive &&
                    request.StartTime < x.EndTime &&
                    request.EndTime > x.StartTime,
                    cancellationToken);

            if (clinicConflict)
                return Result<DoctorScheduleDto>.Failure(ResultStatus.Conflict,
                    "Clinic already has a schedule at this time.");






            var schedule = new doctorSchedule
            {
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Location = request.Location,

                DoctorId = doctor.DoctorId,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CreatedBy = currentUserService.UserName,
                DayOfWeek = (WeekDay)request.Date.DayOfWeek
            };

            await unitOfWork.DoctorSchedules.AddAsync(schedule);
            await unitOfWork.SaveAsync();








            var slots = new List<ScheduleSlot>();

            var current = request.StartTime;

            while (current.AddMinutes(request.SlotDurationInMinutes) <= request.EndTime)
            {
                var end = current.AddMinutes(request.SlotDurationInMinutes);

                slots.Add(new ScheduleSlot
                {

                    DoctorScheduleId = schedule.ScheduleId,
                    StartTime = current,
                    EndTime = end,
                    Status = SlotStatus.Available,
                    CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                    Date = schedule.Date
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

            };
            return Result<DoctorScheduleDto>.Success(scheduleDto);
        }
    }
}
