using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Dtos
{
    public class AppointmentDtoForDoctorDashboard
    {
        public string Title { get; set; }
        public string DoctorName { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; }
    }
}
