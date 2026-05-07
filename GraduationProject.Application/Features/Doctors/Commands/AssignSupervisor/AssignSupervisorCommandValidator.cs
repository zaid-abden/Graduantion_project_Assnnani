using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.AssignSupervisor
{
    public class AssignSupervisorCommandValidator
         : AbstractValidator<AssignSupervisorCommand>
    {
        public AssignSupervisorCommandValidator()
        {
            RuleFor(x => x.StudentDoctorId)
                .GreaterThan(0)
                .WithMessage("Invalid student doctor id.");

            RuleFor(x => x.ClinicName)
                .NotEmpty()
                .WithMessage("Clinic name is required.")
                .MaximumLength(200);

            RuleFor(x => x.ClinicLocation)
                .NotEmpty()
                .WithMessage("Clinic location is required.")
                .MaximumLength(300);

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        }
    }
}
