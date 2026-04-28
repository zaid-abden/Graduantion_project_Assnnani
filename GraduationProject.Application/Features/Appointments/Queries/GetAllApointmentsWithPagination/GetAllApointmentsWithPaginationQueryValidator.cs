using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAllApointmentsWithPagination
{
    public class GetAllAppointmentsWithPaginationQueryValidator
      : AbstractValidator<GetAllApointmentsWithPaginationQuery>
    {
        public GetAllAppointmentsWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize cannot exceed 50.");
        }
    }
}
