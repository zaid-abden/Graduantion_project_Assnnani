using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.ScheduleAppointment
{
    public class ScheduleAppointmentValidator
          : AbstractValidator<ScheduleAppointmentCommand>
    {
        public ScheduleAppointmentValidator()
        {
           
            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .WithMessage("PatientId must be greater than 0");

            
            RuleFor(x => x.slotId)
                .GreaterThan(0)
                .WithMessage("SlotId is required");

            
            RuleFor(x => x.Date)
                .Must(date => date >= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Cannot book an appointment in the past");

          
            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required")
                .MaximumLength(500)
                .WithMessage("Reason must not exceed 500 characters");

          
            RuleFor(x => x.PaymentMethod)
                .IsInEnum()
                .WithMessage("Invalid payment method");

            
            RuleFor(x => x.AppointmentType)
                .IsInEnum()
                .WithMessage("Invalid appointment type");
        }
    }
}
