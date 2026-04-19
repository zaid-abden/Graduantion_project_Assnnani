using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorTodaySummary
{
	public class GetDoctorTodaySummaryValidator : AbstractValidator<GetDoctorTodaySummaryQuery>
	{
		public GetDoctorTodaySummaryValidator()
		{
			RuleFor(x => x.DoctorId)
				.NotEmpty().WithMessage("id is required")
				.GreaterThan(0).WithMessage("id is wrong");
		}
	}
}
