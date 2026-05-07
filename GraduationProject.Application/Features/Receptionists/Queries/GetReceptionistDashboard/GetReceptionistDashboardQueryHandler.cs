using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Receptionists.Dtos;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistDashboard
{
    public class GetReceptionistDashboardQueryHandler : IRequestHandler<GetReceptionistDashboardQuery, Result<ReceptionistDashboardDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetReceptionistDashboardQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<ReceptionistDashboardDto>> Handle(GetReceptionistDashboardQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<ReceptionistDashboardDto>.Failure(ResultStatus.Unauthorized, "You must be logged in to access this resource.");
            var userId = currentUserService.UserId;
            var receptionist = await unitOfWork.Receptionists.Query()

                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (receptionist is null)
                return Result<ReceptionistDashboardDto>.Failure(ResultStatus.NotFound, "No receptionist profile found for this user.");

            var today = DateOnly.FromDateTime(DateTime.Today);

            var appointmentsCount = await unitOfWork.Appointments.Query()
     .Where(x =>
         x.DoctorId == receptionist.DoctorId &&
         x.ScheduleSlot.Date == today &&
         x.AppointmentStatus != AppointmentStatus.Cancelled)
     .CountAsync(cancellationToken);

            //        var inQueueCount = await unitOfWork.Appointments.Query()
            //.Where(x =>
            //    x.DoctorId == receptionist.DoctorId &&
            //    x.ScheduleSlot.Date == today &&
            //    x.IsCheckedIn == true &&
            //    x.AppointmentStatus == AppointmentStatus.Confirmed &&
            //    (x.QueueStatus == QueueStatus.Waiting ||
            //     x.QueueStatus == QueueStatus.InProgress))
            //.CountAsync(cancellationToken);
            var inQueueCount = await unitOfWork.Appointments.Query()
        .Where(x =>
            x.DoctorId == receptionist.DoctorId &&
            x.ScheduleSlot.Date == today &&
             x.IsCheckedIn == true &&
            x.AppointmentStatus == AppointmentStatus.Confirmed &&
            x.AppointmentStatus != AppointmentStatus.Cancelled &&
            (x.QueueStatus == QueueStatus.Waiting ||
             x.QueueStatus == QueueStatus.InProgress))
        .CountAsync(cancellationToken);




            var totalPatients = await unitOfWork.Appointments.Query()
    .Where(x =>
        x.DoctorId == receptionist.DoctorId)
    .Select(x => x.PatientId)
    .Distinct()
    .CountAsync(cancellationToken);


            var result = new ReceptionistDashboardDto
            {
                Appointments = appointmentsCount,
                InQueue = inQueueCount,
                TotalPatients = totalPatients
            };

            return Result<ReceptionistDashboardDto>.Success(result);

        }
    }
}
