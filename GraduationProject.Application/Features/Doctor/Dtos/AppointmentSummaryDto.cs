using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
	public class AppointmentSummaryDto
	{
		public int AppointmentId { get; set; }
		public string PatientName { get; set; }
		public DateTime AppointmentTime { get; set; }
		public string BookingType { get; set; } // Enum (Consultation, Follow-up, etc.)
	}
}
