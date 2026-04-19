using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Financial.DTOs
{
	public class TransactionDto
	{
		public int AppointmentId { get; set; }
		public string PatientName { get; set; } = string.Empty; // منع الـ null
		public DateTime Date { get; set; }
		public decimal Amount { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}
