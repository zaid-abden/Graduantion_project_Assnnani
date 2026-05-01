using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Dtos
{
    public class WeeklyScheduleDto
    {
        public int TotalSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int UnavailableSlots { get; set; }

        public List<DayScheduleDto> Days { get; set; } = new();
    }
}
