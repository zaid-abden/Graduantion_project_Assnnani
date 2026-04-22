using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.RestoreSchedule
{
    public class RestoreScheduleCommandValidator : AbstractValidator<RestoreScheduleCommand>
    {
        public RestoreScheduleCommandValidator()
        {
            RuleFor(x => x.ScheduleId)
                .GreaterThan(0)
                .WithMessage("Invalid Schedule Id");
        }
    }
}
