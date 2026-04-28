using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAvailableDoctorsWithSlots
{
    public class GetAvailableDoctorsWithSlotsQueryValidator
         : AbstractValidator<GetAvailableDoctorsWithSlotsQuery>
    {
        public GetAvailableDoctorsWithSlotsQueryValidator()
        {
            RuleFor(x => x.Date)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date cannot be in the past.");

          

            RuleFor(x => x.Location)
                .MaximumLength(200)
                .WithMessage("Location cannot exceed 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.Location));
        }
    }
}
