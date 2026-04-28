using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatients
{
    public class GetPatientsQueryValidator : AbstractValidator<GetPatientsQuery>
    {
        public GetPatientsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be greater than or equal to 1");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Search))
                .WithMessage("Search term must not exceed 100 characters");

            RuleFor(x => x.PatientStatus)
                .IsInEnum()
                .When(x => x.PatientStatus.HasValue)
                .WithMessage("Invalid patient status");

            RuleFor(x => x.Status)
                .Must(BeValidStatus)
                .When(x => !string.IsNullOrWhiteSpace(x.Status))
                .WithMessage("Status must be 'Active' or 'Inactive'");
        }

        private bool BeValidStatus(string status)
        {
            return status.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                   status.Equals("Inactive", StringComparison.OrdinalIgnoreCase);
        }
    }
}
