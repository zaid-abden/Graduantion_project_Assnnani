using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientDoctorInfo
{
    public class GetPatientDoctorInfoQueryValidator: AbstractValidator<GetPatientDoctorInfoQuery>
    {
        public GetPatientDoctorInfoQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Patient Id must be greater than 0");
        }
    }
}
