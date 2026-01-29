using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctor
{
    public class VerifyEmailCommandValidator:AbstractValidator<VerifyEmailCommand>  
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email is required")
           .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required")
                .Length(4, 10).WithMessage("Verification code length is invalid");
        }
    }
}
