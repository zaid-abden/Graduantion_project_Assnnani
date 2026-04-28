using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecordAttachment
{
    public class AddMedicalRecordAttachmentCommandValidator
         : AbstractValidator<AddMedicalRecordAttachmentCommand>
    {
        public AddMedicalRecordAttachmentCommandValidator()
        {
            RuleFor(x => x.MedicalRecordId)
                .GreaterThan(0)
                .WithMessage("MedicalRecordId must be greater than 0");

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required");

            RuleFor(x => x.File.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024) // 5MB
                .WithMessage("File size must be less than or equal 5MB");
        }
    }
}
