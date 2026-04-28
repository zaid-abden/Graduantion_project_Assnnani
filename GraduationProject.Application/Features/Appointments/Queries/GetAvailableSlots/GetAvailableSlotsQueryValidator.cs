using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAvailableSlots
{
    public class GetAvailableSlotsQueryValidator
       : AbstractValidator<GetAvailableSlotsQuery>
    {
        public GetAvailableSlotsQueryValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .WithMessage("DoctorId must be greater than 0.");

            RuleFor(x => x.Date)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date cannot be in the past.");
        }
    }

}
