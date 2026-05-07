using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetscheduleAppointment
{
    public class GetscheduleAppointmentQueryValidator : AbstractValidator<GetscheduleAppointmentQuery>
    {
        public GetscheduleAppointmentQueryValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("AppointmentId is required.")
                .GreaterThan(0).WithMessage("AppointmentId must be greater than 0.");
        }
    }
}
