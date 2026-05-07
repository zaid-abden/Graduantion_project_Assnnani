using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Receptionists.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientQueue
{
    public class GetPatientQueueQuery : IRequest<Result<List<PatientQueueDto>>>
    {
    }
}
