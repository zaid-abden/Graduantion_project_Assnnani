using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Doctors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfileForPatient
{
    public class GetDoctorProfileForPatientQuery : IRequest<Result<DoctorProfileToPatientDto>>
    {
        public int DoctorId { get; set; }

        public GetDoctorProfileForPatientQuery(int doctorId)
        {
            DoctorId = doctorId;
        }
    }
}
