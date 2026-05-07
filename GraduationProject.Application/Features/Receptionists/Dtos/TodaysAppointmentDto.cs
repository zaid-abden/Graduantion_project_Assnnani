using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Dtos
{
    public class TodaysAppointmentDto
    {
        public int Id { get; set; } 
        public string DoctorName { get; set; }
        public string Specialty { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
    }
}
