using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientInfo
{
    public class GetPatientInfoQuery : IRequest<Result<PatientInfoDto>>
    {
        public int Id { get; set; }

        public GetPatientInfoQuery(int id)
        {
            Id = id;
        }
    }
    public class PatientInfoDto
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }

        public string Status { get; set; }

        public string DoctorName { get; set; }
        public string Specialty { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

        public string AppointmentType { get; set; }

        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}
