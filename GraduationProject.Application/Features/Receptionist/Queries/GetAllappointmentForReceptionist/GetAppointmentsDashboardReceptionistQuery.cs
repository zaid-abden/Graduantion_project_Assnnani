using GraduationProject.Application.Features.Receptionist.Queries.GetAllappointmentForReceptionist.Enums;
using MediatR;

namespace GraduationProject.Application.Features.Receptionist.Queries.GetAllappointmentForReceptionist
{
    public class GetAppointmentsDashboardReceptionistQuery : IRequest<AppointmentsDashboardDto>
    {
        public int ReceptionistId { get; set; }
        public string? Search { get; set; }
        public Status? Status { get; set; }
        public BookingType? Type { get; set; }
    }
}
