using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{

    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
       public AppointmentStatus AppointmentStatus=AppointmentStatus.Confirmed;
        public string Notes { get; set; }
        public BookingType BookingType { get; set; } = BookingType.Online;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        // FK
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorScheduleId { get; set; }
        public doctorSchedule DoctorSchedule { get; set; }
    }

}
