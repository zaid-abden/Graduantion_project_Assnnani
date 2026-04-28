using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetRecentPatients
{
    public class GetRecentPatientsQueryValidator
         : AbstractValidator<GetRecentPatientsQuery>
    {
        public GetRecentPatientsQueryValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0)
                .WithMessage("Count must be greater than 0")
                .LessThanOrEqualTo(50)
                .WithMessage("Count cannot exceed 50");
        }
    }
}
