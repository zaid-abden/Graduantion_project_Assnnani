using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.GetPatientById
{
    public class GetPatientByIdQueryValidation:AbstractValidator<GetPatientByIdQuery>
    {
        public GetPatientByIdQueryValidation()
        {
            RuleFor(x => x.Id)
    .NotEmpty().WithMessage("Patient Id must be provided.")
    .GreaterThan(0).WithMessage("Patient Id must be greater than 0.");
        }
    }
}
