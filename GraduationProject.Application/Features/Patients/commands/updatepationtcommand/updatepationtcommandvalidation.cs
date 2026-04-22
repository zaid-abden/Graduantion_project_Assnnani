
namespace GraduationProject.Application.Features.Patients.commands.updatepationtcommand
{
    using FluentValidation;

    public class UpdatePatientProfileValidator : AbstractValidator<updatepationtcommand>
    {
        public UpdatePatientProfileValidator()
        {
            //RuleFor(x => x.FName)
            //    .NotEmpty().WithMessage("FName is required")
            //    .MaximumLength(100);
            //RuleFor(x => x.LName)
            //    .NotEmpty().WithMessage("LName is required")
            //    .MaximumLength(100);

            //RuleFor(x => x.Phone)
            //    .NotEmpty().WithMessage("Phone is required");

            //RuleFor(x => x.Address)
            //    .MaximumLength(200);

            //RuleFor(x => x.MedicalHistory)
            //    .MaximumLength(1000);
        }
    }
}
