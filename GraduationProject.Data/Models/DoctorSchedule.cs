using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class DoctorSchedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public WeekDay DayOfWeek { get; set; }= WeekDay.Saturday;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // FK
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }

}
