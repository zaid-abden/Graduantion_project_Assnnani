using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Allergies.Commands.AddAllergy
{
    public class AddAllergyCommandValidator : AbstractValidator<AddAllergyCommand>
    {
        public AddAllergyCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Allergy name is required")
                .MaximumLength(100).WithMessage("Allergy name must not exceed 100 characters");
        }
    }
}
