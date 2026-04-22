using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
	public class DoctorTodaySummaryDto
	{
		public int TotalTodayAppointments { get; set; }
		public List<AppointmentSummaryDto> Appointments { get; set; } = new();
	}
}
