using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Receptionist.Queries.GetAllPatients
{
    public class GetPatientsListQuery : IRequest<Result<List<PatientListDtoForReceptionist>>>
    {
        public int ReceptionistId { get; set; }
        public int? DoctorId { get; set; }
        public string? Status { get; set; }
        public string? Search { get; set; }
    }
}
