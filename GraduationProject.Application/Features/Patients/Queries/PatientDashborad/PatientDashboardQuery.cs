using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Patients.Queries.PatientDashborad
{
    public class PatientDashboardQuery : IRequest<Result<PatientDashboardDto>>
    {
        public int PatientId { get; set; }
        public PatientDashboardQuery(int id)
        {
            PatientId = id;
        }
    }
}
