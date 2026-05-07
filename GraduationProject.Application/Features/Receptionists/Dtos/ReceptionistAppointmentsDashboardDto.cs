using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Dtos
{
    public class ReceptionistAppointmentsDashboardDto
    {
        public int Total { get; set; }
        public int Upcoming { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }

        public List<AppointmentItemDto> Appointments { get; set; } = new();
    }
    public class AppointmentItemDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string Type { get; set; } 
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; } 
        public string Mode { get; set; }
    }
}
