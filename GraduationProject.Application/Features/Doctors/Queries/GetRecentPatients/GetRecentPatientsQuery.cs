using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Patients.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetRecentPatients
{
    public class GetRecentPatientsQuery : IRequest<Result<List<RecentPatientDto>>>
    {
     
        public int Count { get; set; } = 5;
    }
}
