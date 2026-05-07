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

namespace GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistOverview
{
    public class GetReceptionistOverviewQueryHandler : IRequestHandler<GetReceptionistOverviewQuery, Result<ReceptionistOverviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetReceptionistOverviewQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this._unitOfWork = unitOfWork;
            this._currentUserService = currentUserService;
        }
        public async Task<Result<ReceptionistOverviewDto>> Handle(
     GetReceptionistOverviewQuery request,
     CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
                return Result<ReceptionistOverviewDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized");

            var userId = _currentUserService.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<ReceptionistOverviewDto>.Failure(
                    ResultStatus.NotFound,
                    "Receptionist not found");

            var today = DateOnly.FromDateTime(DateTime.Today);

            var query = _unitOfWork.Appointments.Query()
                .Where(x => x.DoctorId == receptionist.DoctorId &&
                            x.ScheduleSlot.Date == today);

            var data = await query
                .GroupBy(x => 1)
                .Select(g => new ReceptionistOverviewDto
                {
                    CheckIns = g.Count(x => x.IsCheckedIn),

                    NewRegistrations = g.Count(x =>
                        x.CreatedAt.Date == DateTime.Today),

                    ScheduledAppointments = g.Count(x =>
                        x.AppointmentStatus == AppointmentStatus.Pending ||
                        x.AppointmentStatus == AppointmentStatus.Confirmed),

                    Cancellations = g.Count(x =>
                        x.AppointmentStatus == AppointmentStatus.Cancelled),

                    CompletedAppointments = g.Count(x =>
                        x.AppointmentStatus == AppointmentStatus.Completed),

                    ActiveQueue = g.Count(x =>
                        x.QueueStatus == QueueStatus.Waiting ||
                        x.QueueStatus == QueueStatus.InProgress)
                }

                 )
                .FirstOrDefaultAsync(cancellationToken);

            return Result<ReceptionistOverviewDto>.Success(
                data ?? new ReceptionistOverviewDto()
            );
        }
    }
}
