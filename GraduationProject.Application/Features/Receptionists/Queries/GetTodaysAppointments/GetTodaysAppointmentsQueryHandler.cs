using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Receptionists.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetTodaysAppointments
{
    public class GetTodaysAppointmentsQueryHandler
      : IRequestHandler<GetTodaysAppointmentsQuery, Result<List<TodaysAppointmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetTodaysAppointmentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<TodaysAppointmentDto>>> Handle(
            GetTodaysAppointmentsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<List<TodaysAppointmentDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized");

            var userId = currentUserService.UserId;

            var receptionist = await unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<List<TodaysAppointmentDto>>.Failure(
                    ResultStatus.NotFound,
                    "Receptionist not found");

            var today = DateOnly.FromDateTime(DateTime.Today);

            var result = await unitOfWork.Appointments.Query()
                .Include(x => x.Doctor)
                .Include(x => x.ScheduleSlot)
                .Where(x =>
                    x.DoctorId == receptionist.DoctorId &&
                    x.ScheduleSlot.Date == today)
                .Select(x => new TodaysAppointmentDto
                {
                    Id = x.AppointmentId,
                    DoctorName = x.Doctor.FullName!,
                    Specialty = x.Doctor.Specialization.Name.ToString(),
                    Time = x.ScheduleSlot.StartTime.ToString("hh\\:mm"),
                    Status = x.AppointmentStatus == AppointmentStatus.Confirmed
                        ? "confirmed"
                        : x.AppointmentStatus == AppointmentStatus.Pending
                            ? "pending"
                            : "completed"
                })
                .ToListAsync(cancellationToken);

            return Result<List<TodaysAppointmentDto>>.Success(result);
        }
    }
}
