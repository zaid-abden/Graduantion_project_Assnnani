using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{

    public class DoctorDashboardDto
    {
        public int TodayAppointments { get; set; }
        public int TotalPatients { get; set; }
        public int PendingScans { get; set; }
        public string SuperVisingNumber{ get; set; }
        public double SatisfactionRate { get; set; }
    }
}
