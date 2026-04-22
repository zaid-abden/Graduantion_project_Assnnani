using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.AddAppointment
{
    public class AddAppointmentCommandValidator : AbstractValidator<AddAppointmentCommand>
    {
        public AddAppointmentCommandValidator()
        {
       
            RuleFor(x => x.ScheduleSlotId)
                .NotEmpty().WithMessage("ScheduleSlotId is required")
                .GreaterThan(0).WithMessage("Invalid ScheduleSlotId");

         
            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters")
                .When(x => x.Notes != null);

          
            RuleFor(x => x.PaymentMethod)
                .IsInEnum()
                .WithMessage("Invalid payment method");
        }
    }
}
