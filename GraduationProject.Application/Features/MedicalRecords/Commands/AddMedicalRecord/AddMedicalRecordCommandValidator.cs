using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecord
{
    public class AddMedicalRecordCommandValidator : AbstractValidator<AddMedicalRecordCommand>
    {
        public AddMedicalRecordCommandValidator()
        {

            RuleFor(x => x.Diagnosis)
                .NotEmpty()
                .WithMessage("Diagnosis is required")
                .MaximumLength(300);

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .When(x => x.Notes != null);

          
        }
    }
}
