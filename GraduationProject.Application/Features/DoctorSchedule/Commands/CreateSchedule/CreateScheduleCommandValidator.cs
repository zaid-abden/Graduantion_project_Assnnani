using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.CreateSchedule
{
    public class CreateScheduleCommandValidator
    : AbstractValidator<CreateScheduleCommand>
    {
        public CreateScheduleCommandValidator()
        {
            RuleFor(x => x.DayOfWeek)
                .IsInEnum()
                .WithMessage("DayOfWeek is invalid.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.");
            RuleFor(x => x)
    .Must(x => (x.EndTime - x.StartTime).TotalHours <= 12)
    .WithMessage("Schedule duration cannot exceed 12 hours.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time.");

            RuleFor(x => x.Location)
                .MaximumLength(200)
                .WithMessage("Location must not exceed 200 characters.");

            RuleFor(x => x.MaxAppointments)
                .GreaterThan(0)
                .WithMessage("Max appointments must be greater than zero.");
        }
    }
    }
