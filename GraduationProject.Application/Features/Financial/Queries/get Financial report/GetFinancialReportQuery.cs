using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Financial.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Financial.Queries.get_Financial_report
{
	public record GetFinancialReportQuery(int DoctorId, DateTime StartDate, DateTime EndDate)
	: IRequest<Result<FinancialReportResponse>>; // هنا نحدد نوع الـ Result
}
