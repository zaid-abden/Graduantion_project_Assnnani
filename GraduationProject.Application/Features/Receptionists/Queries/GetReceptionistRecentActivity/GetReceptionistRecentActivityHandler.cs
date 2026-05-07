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
using System.Threading;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistRecentActivity
{
    public class GetReceptionistRecentActivityHandler : IRequestHandler<GetReceptionistRecentActivityQuery, Result<List<ReceptionistActivityDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetReceptionistRecentActivityHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Result<List<ReceptionistActivityDto>>> Handle(
          GetReceptionistRecentActivityQuery request,
          CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
                return Result<List<ReceptionistActivityDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized");

            var userId = _currentUserService.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<List<ReceptionistActivityDto>>.Failure(ResultStatus.Forbidden, "You are not authorized to perform this action.");
            var activities = await _unitOfWork.Appointments.Query()
                .Include(x => x.Patient)
                    .ThenInclude(p => p.User)
                .Where(x => x.DoctorId == receptionist.DoctorId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(20)
                .Select(x => new ReceptionistActivityDto
                {
                    Title =
                        x.AppointmentStatus == AppointmentStatus.Cancelled ? "Appointment cancelled" :
                        x.AppointmentStatus == AppointmentStatus.Completed ? "Appointment completed" :
                        x.QueueStatus == QueueStatus.InProgress ? "Patient in consultation" :
                        x.IsCheckedIn ? "Patient checked in" :
                        "Appointment scheduled",

                    Description = x.Patient.User.FirstName + " " + x.Patient.User.LastName,

                    Time = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<ReceptionistActivityDto>>.Success(activities);
        }

    }
}
