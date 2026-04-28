using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class ScheduleSlot
    {
        public int Id { get; set; }

        public int DoctorScheduleId { get; set; }
        public doctorSchedule DoctorSchedule { get; set; }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly Date { get; set; }
        public SlotStatus Status { get; set; } = SlotStatus.Available;

        public Appointment? Appointment { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
