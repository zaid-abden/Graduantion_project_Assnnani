using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommandValidator : AbstractValidator<RescheduleAppointmentCommand>
    {
        public RescheduleAppointmentCommandValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("AppointmentId is required.")
                .GreaterThan(0).WithMessage("AppointmentId must be greater than 0.");

            RuleFor(x => x.NewSlotId)
                .NotEmpty().WithMessage("NewSlotId is required.")
                .GreaterThan(0).WithMessage("NewSlotId must be greater than 0.");

            RuleFor(x => x.NewDate)
                .Must(date => date >= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("New date must be today or in the future.");
        }
    }
}
