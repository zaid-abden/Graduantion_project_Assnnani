using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.StudentDoctors.Commands.VerifyStudentDoctorEmail
{
    public class VerifyStudentDoctorEmailCommandValidator
    : AbstractValidator<VerifyStudentDoctorEmailCommand>
    {
        public VerifyStudentDoctorEmailCommandValidator()
        {
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

         
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required")
                .Length(4, 10).WithMessage("Code must be between 4 and 10 characters");
        }
    }
}
