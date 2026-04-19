using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Financial.DTOs
{
	public class MonthlyChartData
	{
		public string MonthName { get; set; } = string.Empty;
		public decimal Earnings { get; set; }
	}
}
