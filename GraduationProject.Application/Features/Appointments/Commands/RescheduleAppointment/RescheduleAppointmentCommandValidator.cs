using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommandValidator
        : AbstractValidator<RescheduleAppointmentCommand>
    {
        public RescheduleAppointmentCommandValidator()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0)
                .WithMessage("AppointmentId must be greater than 0.");

            RuleFor(x => x.NewScheduleSlotId)
                .GreaterThan(0)
                .WithMessage("NewScheduleSlotId must be greater than 0.");
        }
    }
}
