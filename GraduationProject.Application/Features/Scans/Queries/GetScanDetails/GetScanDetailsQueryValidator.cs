using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Queries.GetScanDetails
{
    public class GetScanDetailsQueryValidator : AbstractValidator<GetScanDetailsQuery>
    {
        public GetScanDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Scan Id must be greater than 0.");
        }
    }
}
