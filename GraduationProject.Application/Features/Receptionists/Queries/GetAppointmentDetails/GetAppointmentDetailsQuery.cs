using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetAppointmentDetails
{
    public class GetAppointmentDetailsQuery : IRequest<Result<AppointmentDetailsDto>>
    {
        public int Id { get; set; }
        public GetAppointmentDetailsQuery(int id)
        {
            Id = id;
        }
    }
    public class AppointmentDetailsDto
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }

        public string Location { get; set; } 

        public int Duration { get; set; } 
        public string Type { get; set; }

        public string Notes { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }

        public BookingType Mode { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
