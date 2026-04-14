using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Dtos
{
    public class DoctorScheduleDto
    {
        public int ScheduleId { get; set; }
        public WeekDay DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string? Location { get; set; }
        public bool IsActive { get; set; }

        public int MaxAppointments { get; set; }
    }

}
