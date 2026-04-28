using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Patients.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatientInfo
{
    public class GetPatientInfoQuery:IRequest<Result<PatientDetailsDto>>
    {
        public int Id { get; set; }
        public GetPatientInfoQuery(int id)
        {
            Id = id;
        }
    }
}
