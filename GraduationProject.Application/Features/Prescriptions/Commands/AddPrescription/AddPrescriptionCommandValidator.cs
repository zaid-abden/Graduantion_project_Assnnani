using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Prescriptions.Commands.AddPrescription
{
    public class AddPrescriptionCommandValidator : AbstractValidator<AddPrescriptionCommand>
    {
        public AddPrescriptionCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .WithMessage("PatientId must be greater than 0");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Prescription must contain at least one item");

          //  RuleForEach(x => x.Items).SetValidator(new CreatePrescriptionItemDtoValidator());
        }
    }
}
