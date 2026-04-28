using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.ChangePatientStatus
{
    public class ChangePatientStatusValidator : AbstractValidator<ChangePatientStatusCommand>
    {
        public ChangePatientStatusValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .WithMessage("Patient Id must be greater than 0");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid patient status");

        }
    }
}
