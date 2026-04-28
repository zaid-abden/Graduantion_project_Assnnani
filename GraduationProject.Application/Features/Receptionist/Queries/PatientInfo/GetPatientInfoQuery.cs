using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Receptionist.Queries.PatientInfo
{
    public class GetPatientInfoQuery : IRequest<Result<PatientInfoDto>>
    {
        public int PatientId { get; set; }

        public GetPatientInfoQuery(int patientId)
        {
            PatientId = patientId;
        }
    }
}