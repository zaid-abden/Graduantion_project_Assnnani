using FluentValidation;

namespace GraduationProject.Application.Features.Users
{
    internal class RotationTokensValidations : AbstractValidator<RotationTokensCommand>
    {
        public RotationTokensValidations()
        {
            RuleFor(x => x.accussToken)
                .NotEmpty().WithMessage("accussToken is required.")
                .NotNull().WithMessage("accussToken is required.");


            RuleFor(x => x.RefreshToken)
                 .NotEmpty().WithMessage("RefreshToken is required.")
                .NotNull().WithMessage("RefreshToken is required.");
            RuleFor(x => x.UserId)
                 .NotEmpty().WithMessage("UserId is required.")
                .NotNull().WithMessage("UserId is required.");

        }
    }
}
