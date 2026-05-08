using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctorBreak
{
    public class CreateDoctorBreakCommandHandler : IRequestHandler<CreateDoctorBreakCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly INotificationService notificationService;

        public CreateDoctorBreakCommandHandler(IUnitOfWork unitOfWork
            ,ICurrentUserService currentUserService,
            INotificationService notificationService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.notificationService = notificationService;
        }
        public async Task<Result<string>> Handle(CreateDoctorBreakCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(
     ResultStatus.Unauthorized,
     "Please log in first.");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);
            if (doctor == null)
                return Result<string>.Failure(ResultStatus.Forbidden, "You do not have permission to perform this action.");
            var endTime = request.StartTime.AddMinutes(request.DurationInMinutes);

          
          
            var hasOverlap = await unitOfWork.DoctorBreaks.Query()
                .AnyAsync(x =>
                    x.DoctorId == doctor.DoctorId &&
                    !(endTime <= x.StartTime || request.StartTime >= x.EndTime),
                    cancellationToken);

            if (hasOverlap)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "This break overlaps with an existing break.");


            var startTimeOnly = TimeOnly.FromDateTime(request.StartTime);
            var endTimeOnly = TimeOnly.FromDateTime(endTime);

            var hasAppointmentConflict = await unitOfWork.Appointments.Query()
    .Include(a => a.ScheduleSlot)
    .AnyAsync(x =>
        x.DoctorId == doctor.DoctorId &&
        x.ScheduleSlot != null &&
        startTimeOnly < x.ScheduleSlot.EndTime &&
        endTimeOnly > x.ScheduleSlot.StartTime,
        cancellationToken);

            if(hasAppointmentConflict)
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "This break overlaps with an existing appointment.");

            var doctorBreak = new DoctorBreak
            {
                DoctorId = doctor.DoctorId,
                StartTime = request.StartTime,
                EndTime = endTime,
            };

            await unitOfWork.DoctorBreaks.AddAsync(doctorBreak);
            await unitOfWork.SaveAsync();
            var now = TimeOnly.FromDateTime(DateTime.Now);

            var receptionist = await unitOfWork.Receptionists.Query()
                .Where(x =>
                    x.IsActive &&
                    x.ShiftStart <= now &&
                    x.ShiftEnd >= now)
                .FirstOrDefaultAsync(cancellationToken);
            if (receptionist != null)
            {
                await notificationService.SendToUserAsync(
                    receptionist.UserId,
                    "Doctor Break Scheduled",
                    $"Doctor will be on break from {request.StartTime:HH:mm} to {endTime:HH:mm}",
                    NotificationType.Info
                );
            }
            return Result<string>.Success("Break created successfully.");

        }
    }
}
