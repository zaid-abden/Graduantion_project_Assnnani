using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GraduationProject.Data.Models
{
    [Table("DoctorSchedules")]
    public class doctorSchedule:BaseEntity
    {
        [Key]
        public int ScheduleId { get; set; }
       // public WeekDay DayOfWeek { get; set; }= WeekDay.Saturday;
        public WeekDay DayOfWeek {  get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Location { get; set; } 
        public bool IsActive { get; set; } = true;
      public DateOnly Date {  get; set; }
        public int MaxAppointments { get; set; } = 10;
        // FK
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }=null!;
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<ScheduleSlot> Slots { get; set; } = new List<ScheduleSlot>();
    }

}
