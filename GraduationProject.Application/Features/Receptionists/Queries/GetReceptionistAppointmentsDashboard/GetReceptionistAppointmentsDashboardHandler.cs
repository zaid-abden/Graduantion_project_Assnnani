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

namespace GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistAppointmentsDashboard
{
    public class GetReceptionistAppointmentsDashboardQueryHandler
        : IRequestHandler<GetReceptionistAppointmentsDashboardQuery, Result<ReceptionistAppointmentsDashboardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetReceptionistAppointmentsDashboardQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ReceptionistAppointmentsDashboardDto>> Handle(
       GetReceptionistAppointmentsDashboardQuery request,
       CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
                return Result<ReceptionistAppointmentsDashboardDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized");

            var userId = _currentUserService.UserId;

            var receptionist = await _unitOfWork.Receptionists.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (receptionist is null)
                return Result<ReceptionistAppointmentsDashboardDto>.Failure(
                    ResultStatus.NotFound,
                    "Receptionist not found");

            var query = _unitOfWork.Appointments.Query()
                .Include(x => x.Patient)
                    .ThenInclude(p => p.User)
                .Include(x => x.ScheduleSlot)
                .Where(x => x.DoctorId == receptionist.DoctorId);

            
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Patient.User.FirstName.Contains(request.Search) ||
                    x.Patient.User.LastName.Contains(request.Search));
            }

         
            if (request.BookingType.HasValue)
            {
                query = query.Where(x => x.BookingType == request.BookingType.Value);
            }

          
            if (request.AppointmentStatus.HasValue)
            {
               if(request.AppointmentStatus.Value == ReceptionistAppointmentStatus.Upcoming)
                {
                    query = query.Where(x =>
                        x.AppointmentStatus == AppointmentStatus.Pending ||
                        x.AppointmentStatus == AppointmentStatus.Confirmed);
                }
                else if (request.AppointmentStatus.Value == ReceptionistAppointmentStatus.Completed)
                {
                    query = query.Where(x => x.AppointmentStatus == AppointmentStatus.Completed);
                }
                else if (request.AppointmentStatus.Value == ReceptionistAppointmentStatus.Cancelled)
                {
                    query = query.Where(x => x.AppointmentStatus == AppointmentStatus.Cancelled);
                }
            }

            var total = await query.CountAsync(cancellationToken);

            var upcoming = await query.CountAsync(x =>
                x.AppointmentStatus == AppointmentStatus.Pending ||
                x.AppointmentStatus == AppointmentStatus.Confirmed,
                cancellationToken);

            var completed = await query.CountAsync(x =>
                x.AppointmentStatus == AppointmentStatus.Completed,
                cancellationToken);

            var cancelled = await query.CountAsync(x =>
                x.AppointmentStatus == AppointmentStatus.Cancelled,
                cancellationToken);

            var appointments = await query
                .OrderByDescending(x => x.CreatedAt)
                .Take(20)
                .Select(x => new AppointmentItemDto
                {
                    Id = x.AppointmentId,
                    PatientName = x.Patient.User.FirstName + " " + x.Patient.User.LastName,
                    Type = x.AppointmentType.ToString(),
                    Date = x.ScheduleSlot.Date,
                    Time = x.ScheduleSlot.StartTime,
                    Status = x.AppointmentStatus.ToString(),
                    Mode = x.BookingType.ToString()
                })
                .ToListAsync(cancellationToken);

            return Result<ReceptionistAppointmentsDashboardDto>.Success(
                new ReceptionistAppointmentsDashboardDto
                {
                    Total = total,
                    Upcoming = upcoming,
                    Completed = completed,
                    Cancelled = cancelled,
                    Appointments = appointments
                });
        }
        private static bool MatchStatus(
    AppointmentStatus dbStatus,
    ReceptionistAppointmentStatus? filter)
        {
            if (!filter.HasValue)
                return true;

            return filter.Value switch
            {
                ReceptionistAppointmentStatus.Upcoming =>
                    dbStatus == AppointmentStatus.Pending ||
                    dbStatus == AppointmentStatus.Confirmed,

                ReceptionistAppointmentStatus.Completed =>
                    dbStatus == AppointmentStatus.Completed,

                ReceptionistAppointmentStatus.Cancelled =>
                    dbStatus == AppointmentStatus.Cancelled,

                _ => true
            };
        }
    }
}
