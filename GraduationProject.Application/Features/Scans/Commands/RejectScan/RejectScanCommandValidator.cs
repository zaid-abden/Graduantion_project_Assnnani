using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.RejectScan
{
    public class RejectScanCommandValidator:AbstractValidator<RejectScanCommand>
    {
        public RejectScanCommandValidator()
        {
            RuleFor(x => x.ScanId)
            .GreaterThan(0)
            .WithMessage("ScanId must be greater than 0.");
        }
    }
}
