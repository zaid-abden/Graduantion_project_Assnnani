using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Application.Features.Receptionist.Queries.GetAllappointmentForReceptionist
{
    public class GetAppointmentsDashboardHandler : IRequestHandler<GetAppointmentsDashboardReceptionistQuery, AppointmentsDashboardDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAppointmentsDashboardHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AppointmentsDashboardDto> Handle(GetAppointmentsDashboardReceptionistQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Receptionists.GetAllAppointmentForReceptioist(request.ReceptionistId);

            // -----------------------------------
            // Counts
            // -----------------------------------

            var total = await query.CountAsync(cancellationToken);

            var upcoming = await query.CountAsync(a =>
                    a.AppointmentStatus == AppointmentStatus.UPcoming,
                    cancellationToken);

            var completed = await query.CountAsync(a =>
                    a.AppointmentStatus == AppointmentStatus.Completed,
                    cancellationToken);

            var cancelled = await query.CountAsync(a =>
                    a.AppointmentStatus == AppointmentStatus.Cancelled,
                    cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(a =>
                    a.Patient.User.FullName.Contains(request.Search));
                // || a.title.Contains(request.Search));
            }

            // -----------------------------------
            // Status Filter
            // -----------------------------------

            if (request.Status.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentStatus == request.Status.Value);
            }

            // -----------------------------------
            // Booking Type Filter
            // -----------------------------------

            if (request.Type.HasValue)
            {
                query = query.Where(a =>
                    a.BookingType == request.Type.Value);
            }



            // -----------------------------------
            // Appointments List
            // -----------------------------------

            var appointments = await query
                .OrderByDescending(a => a.ScheduleSlot.Date)
                .ThenBy(a => a.ScheduleSlot.StartTime)

                .Select(a => new AppointmentCardDto
                {
                    AppointmentId = a.AppointmentId,

                    PatientName = a.Patient.User.FullName,

                    //Title = a.Title,

                    Status = a.AppointmentStatus.ToString(),

                    Date = a.ScheduleSlot.Date,

                    Time = a.ScheduleSlot.StartTime,

                    Type = a.BookingType.ToString(),

                    Location = a.Doctor.City
                })
                .ToListAsync(cancellationToken);

            // -----------------------------------
            // Final Response
            // -----------------------------------

            return new AppointmentsDashboardDto
            {
                Total = total,
                Upcoming = upcoming,
                Completed = completed,
                Cancelled = cancelled,
                Appointments = appointments
            };
        }
    }
}
