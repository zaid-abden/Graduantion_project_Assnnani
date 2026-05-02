using FluentValidation;

namespace GraduationProject.Application.Features.Admin.Commands.RejectUser
{
	public class RejectUserValidator : AbstractValidator<RejectUserCommand>
	{
		public RejectUserValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("User ID is required.");
			RuleFor(x => x.Reason)
				.NotEmpty().WithMessage("Rejection reason is required.")
				.MinimumLength(10).WithMessage("Please provide a reason with at least 10 characters.");
		}
	}
}
