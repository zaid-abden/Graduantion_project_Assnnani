using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;

namespace GraduationProject.Application.Features.Patients.Queries.GetAllAppointment
{
    public class GetPatientAppointmentsQuery
      : IRequest<Result<PatientAppointmentsResponse>>
    {
        public int PatientId { get; set; }

        public string? Search { get; set; }

        public AppointmentStatus? Status { get; set; }

        public AppointmentType? Type { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
