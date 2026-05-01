using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.GetAllAppointment
{
    public class PatientAppointmentDto
    {
        public int AppointmentId { get; set; }

        public string DoctorName { get; set; }

        public string DoctorImage { get; set; }

        public string Title { get; set; }

        public AppointmentStatus Status { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public AppointmentType Type { get; set; }

        public string? Location { get; set; }
    }
}
