using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.CompleteAppointment
{
    public class CompleteAppointmentCommandValidator
     : AbstractValidator<CompleteAppointmentCommand>
    {
        public CompleteAppointmentCommandValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("AppointmentId is required.")
                .GreaterThan(0).WithMessage("AppointmentId must be greater than 0.");
        }
    }
}
