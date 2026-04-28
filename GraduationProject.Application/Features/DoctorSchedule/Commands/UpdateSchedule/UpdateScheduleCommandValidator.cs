using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.UpdateSchedule
{
    public class UpdateScheduleCommandValidator : AbstractValidator<UpdateScheduleCommand>
    {
        public UpdateScheduleCommandValidator()
        {
            
            RuleFor(x => x.ScheduleId)
                .GreaterThan(0)
                .WithMessage("ScheduleId must be greater than zero.");

            
           

          
            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.")
                .Must(t => t.TotalSeconds >= 0)
                .WithMessage("Start time cannot be negative.");

           
            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time.")
                .Must(t => t.TotalSeconds >= 0)
                .WithMessage("End time cannot be negative.");

            RuleFor(x => x)
     .Must(x => (x.EndTime - x.StartTime).TotalHours <= 12)
     .WithMessage("Schedule duration cannot exceed 12 hours.");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Location is required.")
                .MaximumLength(200)
                .WithMessage("Location must not exceed 200 characters.");

            
            RuleFor(x => x.MaxAppointments)
                .GreaterThan(0)
                .WithMessage("Max appointments must be greater than zero.");
        }
    }
}
