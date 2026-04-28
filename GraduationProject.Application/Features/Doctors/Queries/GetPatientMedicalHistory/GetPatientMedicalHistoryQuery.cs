using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.MedicalRecords.Commands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatientMedicalHistory
{
    public class GetPatientMedicalHistoryQuery : IRequest<Result<List<MedicalRecordForDoctorDashboardDto>>>
    {
        public int PatientId { get; set; }
    }
}
