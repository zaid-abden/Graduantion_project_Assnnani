using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Auth.Commands.UnlockUser
{
    public class UnlockUserCommandValidator : AbstractValidator<UnlockUserCommand>
    {
        public UnlockUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MinimumLength(10).WithMessage("UserId seems too short.")
                .MaximumLength(100).WithMessage("UserId seems too long.");
        }
    }
}
