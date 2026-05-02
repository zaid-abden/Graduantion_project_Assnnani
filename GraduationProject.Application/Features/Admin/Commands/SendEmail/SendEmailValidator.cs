using FluentValidation;

namespace GraduationProject.Application.Features.Admin.Commands.SendEmail
{
	public class SendEmailValidator : AbstractValidator<SendEmailCommand>
	{
		public SendEmailValidator()
		{
			RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Body).NotEmpty();

			// التأكد أن الأدمن اختار وجهة واحدة على الأقل
			RuleFor(x => x)
				.Must(x => !string.IsNullOrEmpty(x.UserId) || !string.IsNullOrEmpty(x.RoleName) || (x.UserIds != null && x.UserIds.Any()))
				.WithMessage("You must specify a recipient (User, Role, or List of Users).");
		}
	}
}
