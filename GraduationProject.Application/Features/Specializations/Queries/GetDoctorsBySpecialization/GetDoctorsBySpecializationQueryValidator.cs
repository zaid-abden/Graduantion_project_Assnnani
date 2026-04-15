using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetDoctorsBySpecialization
{
    public class GetDoctorsBySpecializationQueryValidator
         : AbstractValidator<GetDoctorsBySpecializationQuery>
    {
        public GetDoctorsBySpecializationQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Specialization Id must be greater than zero.");
        }
    }
}
