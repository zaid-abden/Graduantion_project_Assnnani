using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctorBreak
{
    public class CreateDoctorBreakCommandValidator
     : AbstractValidator<CreateDoctorBreakCommand>
    {
        public CreateDoctorBreakCommandValidator()
        {
           

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.")
                .GreaterThan(DateTime.Now.AddMinutes(-1))
                .WithMessage("Start time must be in the future.");

            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0)
                .LessThanOrEqualTo(60)
                .WithMessage("Duration must be between 1 and 60 minutes.");

            RuleFor(x => x.Note)
                .MaximumLength(250)
                .When(x => x.Note != null);
        }
    }
}
