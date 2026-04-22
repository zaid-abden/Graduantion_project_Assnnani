using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{

    public class Appointment : BaseEntity
    {
        public int AppointmentId { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Pending;

        public string? Notes { get; set; }

        public BookingType BookingType { get; set; } = BookingType.Online;

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

       
        public int ScheduleSlotId { get; set; }
        public ScheduleSlot ScheduleSlot { get; set; } = null!;
    }

}
