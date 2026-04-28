using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.MarkAsArrived
{
    public class MarkAsArrivedCommandValidator : AbstractValidator<MarkAsArrivedCommand>
    {
        public MarkAsArrivedCommandValidator()
        {
            RuleFor(x => x.AppointmentId)
                .GreaterThan(0)
                .WithMessage("AppointmentId must be greater than 0");
        }
    }

}
