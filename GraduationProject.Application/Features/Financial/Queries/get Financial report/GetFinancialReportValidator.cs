using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Financial.Queries.get_Financial_report
{
	public class GetFinancialReportValidator : AbstractValidator<GetFinancialReportQuery>
	{
		public GetFinancialReportValidator()
		{
			RuleFor(x => x.StartDate).NotEmpty().WithMessage("تاريخ البداية مطلوب");
			RuleFor(x => x.EndDate).NotEmpty().WithMessage("تاريخ النهاية مطلوب")
				.GreaterThan(x => x.StartDate).WithMessage("تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
		}
	}
}
