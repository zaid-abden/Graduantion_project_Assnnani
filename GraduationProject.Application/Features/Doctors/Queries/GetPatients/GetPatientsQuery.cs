using GraduationProject.Application.Common.PaginatedResults;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatients
{
    public class GetPatientsQuery : IRequest<Result<PaginatedResult<PatientForDoctorDashboardDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public PatientStatusForDoctorDashboard? PatientStatus { get; set; }
        public string? Status { get; set; }
      


    }
}
