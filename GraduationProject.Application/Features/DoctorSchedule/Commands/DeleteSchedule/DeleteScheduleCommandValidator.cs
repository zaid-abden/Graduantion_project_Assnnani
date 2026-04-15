using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeleteSchedule
{
    public class DeleteScheduleCommandValidator : AbstractValidator<DeleteScheduleCommand>
    {
        public DeleteScheduleCommandValidator()
        {
            RuleFor(x => x.ScheduleId)
                .GreaterThan(0)
                .WithMessage("ScheduleId must be greater than zero.");
        }
    }
}
