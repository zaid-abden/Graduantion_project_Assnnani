using FluentValidation;

namespace GraduationProject.Application.Features.Admin.Queries.GetAllUsers
{
	public class GetAllUsersValidator : AbstractValidator<GetAllUsersQuery>
	{
		public GetAllUsersValidator()
		{
			RuleFor(x => x.PageNumber).GreaterThan(0);
			RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

			RuleFor(x => x.Gender)
				.Must(g => string.IsNullOrEmpty(g) ||
						   g.Equals("Male", StringComparison.OrdinalIgnoreCase) ||
						   g.Equals("Female", StringComparison.OrdinalIgnoreCase))
				.WithMessage("Gender must be 'Male' or 'Female'.");
		}
	}
}
