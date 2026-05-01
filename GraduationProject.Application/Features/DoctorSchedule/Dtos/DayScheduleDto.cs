using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Dtos
{
    public class DayScheduleDto
    {
        public WeekDay Day { get; set; }
        public List<SlotDto> Slots { get; set; } = new();
    }
}
