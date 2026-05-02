using FluentValidation;

namespace GraduationProject.Application.Features.Admin.Queries.GetStats
{
	public class GetDashboardStatsValidator : AbstractValidator<GetDashboardStatsQuery>
	{
		public GetDashboardStatsValidator()
		{
			// No parameters to validate
		}
	}
}
