using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Financial.DTOs
{
	public class FinancialReportResponse
	{
		public decimal TotalEarnings { get; set; }
		public int TotalCompletedAppointments { get; set; }
		public List<MonthlyChartData> ChartData { get; set; } = new(); // قيمة افتراضية
		public List<TransactionDto> TransactionList { get; set; } = new(); // قيمة افتراضية
	}
}
