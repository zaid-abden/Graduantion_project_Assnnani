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
    [Table("DoctorSchedules")]
    public class doctorSchedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public WeekDay DayOfWeek { get; set; }= WeekDay.Saturday;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Location { get; set; } 
        public bool IsActive { get; set; } = true;
        public int MaxAppointments { get; set; } = 10;
        // FK
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }=null!;
        public ICollection<Appointment> Appointments { get; set; }
    }

}
