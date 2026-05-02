using FluentValidation;
using GraduationProject.Application.Features.Admin.Commands.RejectUser;

namespace GraduationProject.Application.Features.Admin.Commands.VerifyUser
{
	public class VerifyUserValidator : AbstractValidator<VerifyUserCommand>
	{
		public VerifyUserValidator()
		{
			RuleFor(x => x.Id)
				.NotEmpty().WithMessage("User ID is required.");

			RuleFor(x => x.Note)
				.MaximumLength(500).WithMessage("Note cannot exceed 500 characters.");
		}
	}
}
