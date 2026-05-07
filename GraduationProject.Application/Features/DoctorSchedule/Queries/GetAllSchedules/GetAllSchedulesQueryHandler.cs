using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllSchedules
{
    public class GetAllSchedulesQueryHandler : IRequestHandler<GetAllSchedulesQuery, Result<List<DoctorScheduleDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllSchedulesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<DoctorScheduleDto>>> Handle(GetAllSchedulesQuery request, CancellationToken cancellationToken)
        {

            if (!currentUserService.IsAuthenticated)
                return Result<List<DoctorScheduleDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not authorized to perform this action.");

            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == currentUserService.UserId, cancellationToken);

            if (doctor == null)
                return Result<List<DoctorScheduleDto>>.Failure(
                    ResultStatus.Failure,
                    "Doctor profile not found.");

            var schedules = await unitOfWork.DoctorSchedules.Query()
                .Where(x => x.DoctorId == doctor.DoctorId)
                .ToListAsync(cancellationToken);

            var slots = await unitOfWork.ScheduleSlots.Query()
                .Include(x => x.DoctorSchedule)
                .Where(x => x.DoctorSchedule.DoctorId == doctor.DoctorId)
                .ToListAsync(cancellationToken);

            var appointments = await unitOfWork.Appointments.Query()
                .Where(x => x.DoctorId == doctor.DoctorId)
                .ToListAsync(cancellationToken);

            var result = slots
                .GroupBy(x => x.DoctorSchedule.DayOfWeek)
                .Select(dayGroup => new DoctorScheduleDto
                {
                    Day = dayGroup.Key.ToString(),

                    Slots = dayGroup.Select(slot =>
                    {
                       

                        return new TimeSlotDto
                        {
                            Id = slot.Id,
                            Start = slot.StartTime,
                            End = slot.EndTime,
                            Status = slot.Status.ToString()
                        };
                    }).ToList()
                })
                .ToList();

            return Result<List<DoctorScheduleDto>>.Success(result);




            //      if (!currentUserService.IsAuthenticated)
            //      {
            //          return Result<List<DoctorScheduleDto>>.Failure(ResultStatus.Unauthorized, "You are not authorized to perform this action.");
            //      }
            //      var userId = currentUserService.UserId;
            //      var doctor = await unitOfWork.Doctors.Query()
            //          .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            //      if (doctor == null)
            //      {
            //          return Result<List<DoctorScheduleDto>>.Failure(ResultStatus.Failure, "Doctor profile not found.");
            //      }

            //      var schedules = await unitOfWork.DoctorSchedules.Query()
            //.Where(x => x.DoctorId == doctor.DoctorId)
            //.ToListAsync(cancellationToken);

            //      var appointments = await unitOfWork.Appointments.Query()
            //          .Where(x => x.DoctorId == doctor.DoctorId)
            //          .ToListAsync(cancellationToken);

            //      var result = schedules
            //          .GroupBy(x => x.DayOfWeek)
            //          .Select(dayGroup => new DoctorScheduleDto
            //          {
            //              Day = dayGroup.Key.ToString(),

            //              Slots = dayGroup.Select(slot => new TimeSlotDto
            //              {
            //                  Start = slot.StartTime,
            //                  End = slot.EndTime,

            //                  IsAvailable = !appointments.Any(a =>
            //                      a.ScheduleSlotId == slot.ScheduleId)
            //              }).ToList()
            //          })
            //          .ToList();

            //      return Result<List<DoctorScheduleDto>>.Success(result);


        }
    }
}

