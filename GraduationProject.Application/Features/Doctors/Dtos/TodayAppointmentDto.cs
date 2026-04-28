using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
    public class TodayAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = null!;
        public string? Specialty { get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; } = null!;
    }
}
