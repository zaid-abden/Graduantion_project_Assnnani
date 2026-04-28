using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPatientInfo
{
    public class GetPatientInfoQueryValidator : AbstractValidator<GetPatientInfoQuery>
    {
        public GetPatientInfoQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Patient Id must be greater than 0");
        }
    }
}
