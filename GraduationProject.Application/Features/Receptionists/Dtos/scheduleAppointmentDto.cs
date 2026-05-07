using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Dtos
{
    public class scheduleAppointmentDto
    {
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }
    }
}
