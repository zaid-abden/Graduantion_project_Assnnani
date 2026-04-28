using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Dtos
{
    public class AddAppointmentResponseDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int ScheduleSlotId { get; set; }

       
        public DateTime AppointmentTime { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }

        public BookingType BookingType { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
