using FluentValidation;

namespace GraduationProject.Application.Features.Admin.Queries.GetFilteredPendingDoctors
{
	public class FilterPendingDoctorsValidator : AbstractValidator<FilterPendingDoctorsQuery>
	{
		public FilterPendingDoctorsValidator()
		{
			RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
			RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
		}
	}
}
