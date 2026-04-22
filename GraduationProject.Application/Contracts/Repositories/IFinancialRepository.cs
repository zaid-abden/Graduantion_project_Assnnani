using GraduationProject.Application.Features.Financial.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Repositories
{
	public interface IFinancialRepository
	{
		Task<FinancialReportResponse> GetDoctorFinancialReportAsync(int doctorId, DateTime start, DateTime end);
	}
}
