using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetSupervisingRequestById
{
    public class GetSupervisingRequestByIdQueryValidator
        : AbstractValidator<GetSupervisingRequestByIdQuery>
    {
        public GetSupervisingRequestByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid supervising request id.");
        }
    }
}
