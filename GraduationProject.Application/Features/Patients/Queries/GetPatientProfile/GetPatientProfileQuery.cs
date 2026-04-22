using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Patients.Queries.GetPatientProfile
{
    public class GetPatientProfileQuery : IRequest<Result<PatientProfileDto>>
    {
        public int UserId { get; set; }  
    }
}
