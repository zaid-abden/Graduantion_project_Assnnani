using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Application.Features.Patients.Queries.GetAllAppointment
{
    public class GetPatientAppointmentsHandler
      : IRequestHandler<GetPatientAppointmentsQuery, Result<PatientAppointmentsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public GetPatientAppointmentsHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Result<PatientAppointmentsResponse>> Handle(GetPatientAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Patients.GetAppointmentsByPatientId(request.PatientId);

            //counts
            var querycount = query.Count();
            var UpcomingCount = await _unitOfWork.Patients.GetAllAppointmentUpcoming(request.PatientId);
            var CompletedCount = await _unitOfWork.Patients.GetAllAppointmentCompleted(request.PatientId);
            var CancelledCount = await _unitOfWork.Patients.GetAllAppointmentCancelled(request.PatientId);

            // 🔥 Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(a =>
                    a.Doctor.User.FullName.Contains(request.Search));
            }

            // 🔥 Status filter
            if (request.Status.HasValue)
            {
                query = query.Where(a => a.AppointmentStatus == request.Status.Value);
            }

            // 🔥 Type filter
            if (request.Type.HasValue)
            {
                query = query.Where(a => a.AppointmentType == request.Type.Value);
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new PatientAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    DoctorName = a.Doctor.User.FullName,
                    DoctorImage = a.Doctor.ImageUrl,
                    Title = "Default",
                    Status = a.AppointmentStatus,
                    Date = a.ScheduleSlot.Date,
                    Time = a.ScheduleSlot.StartTime,
                    Type = a.AppointmentType,
                    Location = a.Doctor.City // this is default it should be room(...)
                })
                .ToListAsync(cancellationToken);

            var paged = new PagedResult<PatientAppointmentDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)request.PageSize)
            };

            var response = new PatientAppointmentsResponse
            {
                TotalAppointments = querycount,
                UpcomingAppointments = UpcomingCount,
                CompletedAppointments = CompletedCount,
                CancelledAppointments = CancelledCount,
                Appointments = paged
            };

            return Result<PatientAppointmentsResponse>.Success(response);
        }
    }
}
