using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.AddPatientAllergy
{
    public class AddPatientAllergyCommandValidator : AbstractValidator<AddPatientAllergyCommand>
    {
        public AddPatientAllergyCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0).WithMessage("PatientId must be greater than 0");

            RuleFor(x => x.AllergyId)
                .GreaterThan(0).WithMessage("AllergyId must be greater than 0");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes must not exceed 500 characters")
                .When(x => x.Notes is not null);
        }
    }
}
