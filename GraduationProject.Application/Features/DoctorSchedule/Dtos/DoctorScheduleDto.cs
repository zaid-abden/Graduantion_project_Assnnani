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
        public string Day { get; set; }
        public List<TimeSlotDto> Slots { get; set; }
    }
    public class TimeSlotDto
    {
        public int Id { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public string Status { get; set; }
    }

}
