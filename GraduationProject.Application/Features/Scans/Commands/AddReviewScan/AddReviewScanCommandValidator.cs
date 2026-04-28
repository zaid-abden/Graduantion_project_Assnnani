using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.AddReviewScan
{
    public class AddReviewScanCommandValidator : AbstractValidator<AddReviewScanCommand>
    {
        public AddReviewScanCommandValidator()
        {
        
            RuleFor(x => x.ScanId)
                .GreaterThan(0)
                .WithMessage("ScanId must be greater than 0.");

        
            RuleFor(x => x.Findings)
                .NotEmpty()
                .WithMessage("Findings is required.")
                .MaximumLength(2000)
                .WithMessage("Findings cannot exceed 2000 characters.");

         
            RuleFor(x => x.Recommendations)
                .NotEmpty()
                .WithMessage("Recommendations is required.")
                .MaximumLength(2000)
                .WithMessage("Recommendations cannot exceed 2000 characters.");
        }
    }
}
