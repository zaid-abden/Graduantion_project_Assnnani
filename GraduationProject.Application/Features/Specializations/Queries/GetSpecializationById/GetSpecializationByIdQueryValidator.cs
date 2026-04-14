using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetSpecializationById
{
    public class GetSpecializationByIdQueryValidator
         : AbstractValidator<GetSpecializationByIdQuery>
    {
        public GetSpecializationByIdQueryValidator()
        {
            RuleFor(x => x)
                .NotNull().WithMessage("Query cannot be null"); 

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Specialization ID must be a positive number");
        }
    }
}
