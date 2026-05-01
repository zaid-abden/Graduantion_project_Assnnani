using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetWeeklySchedule
{
    public class GetWeeklyScheduleQueryHandler : IRequestHandler<GetWeeklyScheduleQuery, Result<WeeklyScheduleDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetWeeklyScheduleQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<WeeklyScheduleDto>> Handle(GetWeeklyScheduleQuery request, CancellationToken cancellationToken)
        {


            if (!currentUserService.IsAuthenticated)
                return Result<WeeklyScheduleDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized access. Please log in");

            var userId = currentUserService.UserId;

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
                return Result<WeeklyScheduleDto>.Failure(
                    ResultStatus.NotFound,
                    "Doctor profile not found");

            var data = await unitOfWork.DoctorSchedules.Query()
                .Where(x =>
                    x.DoctorId == doctor.DoctorId &&
                    x.IsActive &&
                    !x.IsDeleted)
                .ToListAsync(cancellationToken);

            var allDays = new[]
            {
        WeekDay.Monday,
        WeekDay.Tuesday,
        WeekDay.Wednesday,
        WeekDay.Thursday,
        WeekDay.Friday,
        WeekDay.Saturday,
        WeekDay.Sunday
    };

            var schedule = allDays.Select(day => new DayScheduleDto
            {
                Day = day,

                Slots = data
                    .Where(x => x.DayOfWeek == day) 
                    .OrderBy(x => x.StartTime)
                    .Select(slot => new SlotDto
                    {
                        Id = slot.ScheduleId,
                        StartTime = slot.StartTime,  
                        EndTime = slot.EndTime,
                        IsAvailable = slot.IsActive
                    })
                    .ToList()

            }).ToList();

            var result = new WeeklyScheduleDto
            {
                TotalSlots = data.Count,
                AvailableSlots = data.Count(x => x.IsActive),
                UnavailableSlots = data.Count(x => !x.IsActive),

                Days = schedule
            };

            return Result<WeeklyScheduleDto>.Success(result);



            //        {
            //            if (!currentUserService.IsAuthenticated)
            //                return Result<WeeklyScheduleDto>.Failure(
            //                    ResultStatus.Unauthorized,
            //                    "Unauthorized access. Please log in");

            //            var userId = currentUserService.UserId;

            //            var doctor = await unitOfWork.Doctors.Query()
            //                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            //            if (doctor is null)
            //                return Result<WeeklyScheduleDto>.Failure(
            //                    ResultStatus.NotFound,
            //                    "Doctor profile not found");

            //            var data = await unitOfWork.DoctorSchedules.Query()
            //                .Where(x =>
            //                    x.DoctorId == doctor.DoctorId &&
            //                    x.IsActive &&
            //                    !x.IsDeleted)
            //                .OrderBy(x => x.DayOfWeek)
            //                .ThenBy(x => x.StartTime)
            //                .ToListAsync(cancellationToken);

            //            var allDays = new[]
            //            {
            //    DayOfWeek.Monday,
            //    DayOfWeek.Tuesday,
            //    DayOfWeek.Wednesday,
            //    DayOfWeek.Thursday,
            //    DayOfWeek.Friday,
            //    DayOfWeek.Saturday,
            //    DayOfWeek.Sunday
            //};

            //            var schedule = allDays.Select(day => new DayScheduleDto
            //            {
            //                Day = day.ToString(),

            //                Slots = data
            //                    .Where(x => x.DayOfWeek.ToString() == day.ToString())
            //                    .OrderBy(x => x.StartTime)
            //                    .Select(slot => new TimeSlotDto
            //                    {
            //                        Id = slot.ScheduleId,
            //                        StartTime = slot.StartTime.ToString(@"hh\:mm"),
            //                        EndTime = slot.EndTime.ToString(@"hh\:mm")
            //                    })
            //                    .ToList()

            //            }).ToList();

            //            var result = new WeeklyScheduleDto
            //            {
            //                Schedule = schedule
            //            };

            //            return Result<WeeklyScheduleDto>.Success(result);


        }
    }
}