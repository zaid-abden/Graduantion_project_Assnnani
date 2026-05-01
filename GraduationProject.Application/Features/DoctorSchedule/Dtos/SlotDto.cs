using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Dtos
{
    public class SlotDto
    {
        public int Id { get; set; }

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public int Duration => (int)(EndTime - StartTime).TotalMinutes;

        public bool IsAvailable { get; set; }
    }
}
