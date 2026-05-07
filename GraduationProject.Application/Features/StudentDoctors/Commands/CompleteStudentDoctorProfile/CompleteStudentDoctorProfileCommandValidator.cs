using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.StudentDoctors.Commands.CompleteStudentDoctorProfile
{
    public class CompleteStudentDoctorProfileCommandValidator
    : AbstractValidator<CompleteStudentDoctorProfileCommand>
    {
        public CompleteStudentDoctorProfileCommandValidator()
        {
            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

           
            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("National ID is required")
                .Length(14).WithMessage("National ID must be exactly 14 digits")
                .Matches(@"^\d{14}$").WithMessage("National ID must contain only numbers");

         
            RuleFor(x => x.YearsOfStudy)
                .GreaterThan(0).WithMessage("Years of study must be greater than 0")
                .LessThanOrEqualTo(10).WithMessage("Years of study seems invalid");

           
            RuleFor(x => x.SupervisingNumber)
                .NotEmpty().WithMessage("Supervising number is required")
                .MinimumLength(3).WithMessage("Supervising number is too short");

           
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required");
        }
    }
}
