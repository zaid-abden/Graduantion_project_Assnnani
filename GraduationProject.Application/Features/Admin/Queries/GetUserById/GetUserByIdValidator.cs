using FluentValidation;

namespace GraduationProject.Application.Features.Admin.Queries.GetUserById
{
	public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
	{
		public GetUserByIdValidator()
		{
			RuleFor(x => x.UserId).NotEmpty().WithMessage("Requiered Id of user");
		}
	}
}
