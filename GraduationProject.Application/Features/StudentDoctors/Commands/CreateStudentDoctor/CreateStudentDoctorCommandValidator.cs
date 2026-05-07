using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.StudentDoctors.Commands.CreateStudentDoctor
{
    public class CreateStudentDoctorCommandValidator
    : AbstractValidator<CreateStudentDoctorCommand>
    {
        public CreateStudentDoctorCommandValidator()
        {
            // First Name
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MinimumLength(2).WithMessage("FirstName must be at least 2 characters");

           
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MinimumLength(2);

         
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid email format");

           
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
                .WithMessage("Invalid Egyptian phone number");

         
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters");

           
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Password and ConfirmPassword do not match");

        
        }
    }
}
