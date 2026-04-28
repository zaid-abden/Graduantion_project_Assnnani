using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorDashboard
{
    public class GetDoctorDashboardQueryHandler : IRequestHandler<GetDoctorDashboardQuery, Result<DoctorDashboardDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetDoctorDashboardQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<DoctorDashboardDto>> Handle(GetDoctorDashboardQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<DoctorDashboardDto>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId,cancellationToken);
            if (doctor is null)
                return Result<DoctorDashboardDto>.Failure(ResultStatus.NotFound, "Doctor profile not found");
            var today = DateOnly.FromDateTime(DateTime.Today);
            var todayAppointments = await unitOfWork.Appointments.Query()
                .Include(c => c.ScheduleSlot)
                .Where(x => x.DoctorId == doctor.DoctorId
                && !x.IsDeleted && x.ScheduleSlot.Date == today)
                
                .CountAsync(cancellationToken);
           
            var totalPatients = await unitOfWork.Appointments.Query()
      .Where(c => c.DoctorId == doctor.DoctorId
        && c.AppointmentStatus == AppointmentStatus.Completed
          )
      .Select(c => c.PatientId)
      .Distinct()
      .CountAsync(cancellationToken);

            var pendingScans = await unitOfWork.Scans.Query()
                .Where(c => c.DoctorId == doctor.DoctorId
                && c.Status == ScanStatus.Pending)
                .CountAsync(cancellationToken);

            var ratings = unitOfWork.Feedbacks.Query()
      .Where(r => r.DoctorId == doctor.DoctorId);

            var satisfactionRate = await ratings.AnyAsync(cancellationToken)
                ? await ratings.AverageAsync(r => (double)r.Rating, cancellationToken)
                : 0;
            var dto = new DoctorDashboardDto
            {
                TodayAppointments = todayAppointments,
                TotalPatients = totalPatients,
                PendingScans = pendingScans,
                SatisfactionRate = Math.Round(satisfactionRate * 20, 2) 
            };

            return Result<DoctorDashboardDto>.Success(dto);
        }
    }
}
