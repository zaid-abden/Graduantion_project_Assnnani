using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctor
{
    public class SubmitDoctorVerificationCommandValidator
    : AbstractValidator<SubmitDoctorVerificationCommand>
    {
        public SubmitDoctorVerificationCommandValidator()
        {
            RuleFor(x => x.MedicalLicenseNumber)
                .NotNull().WithMessage("Medical license number is required")
                .NotEmpty().WithMessage("Medical license number cannot be empty");

            RuleFor(x => x.NationalId)
     .NotNull().WithMessage("National ID is required")
     .NotEmpty().WithMessage("National ID cannot be empty")
     .Matches(@"^\d{14}$")
     .WithMessage("National ID must be exactly 14 digits");


            RuleFor(x => x.SpecializationId)
                .GreaterThan(0).WithMessage("Specialization ID must be a positive number");

            RuleFor(x => x.YearsOfExperience)
                .InclusiveBetween(0, 50)
                .WithMessage("Years of experience must be between 0 and 50");

            RuleFor(x => x.ClinicName)
                .NotNull().WithMessage("Clinic name is required")
                .NotEmpty().WithMessage("Clinic name cannot be empty");

            RuleFor(x => x.ClinicAddress)
                .NotNull().WithMessage("Clinic address is required")
                .NotEmpty().WithMessage("Clinic address cannot be empty");

            RuleFor(x => x.ClinicPhone)
                .NotNull().WithMessage("Clinic phone is required")
                .NotEmpty().WithMessage("Clinic phone cannot be empty")
                .Matches(@"^\d{11}$")
                .WithMessage("Clinic phone must be 11 digits");

            RuleFor(x => x.Certificate)
                .NotNull().WithMessage("Certificate file is required")
                .Must(c => c.Length > 0)
                .WithMessage("Certificate file cannot be empty");
        }
    }
}
