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
            RuleFor(x => x.Date)
     .NotEmpty()
     .WithMessage("Date is required")
     .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
     .WithMessage("Date cannot be in the past");

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

        }
    }
    }
