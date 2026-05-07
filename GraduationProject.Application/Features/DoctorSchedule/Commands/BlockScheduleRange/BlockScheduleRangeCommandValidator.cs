using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.BlockScheduleRange
{
    public class BlockScheduleRangeCommandValidator
    : AbstractValidator<BlockScheduleRangeCommand>
    {
        public BlockScheduleRangeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Start)
                .LessThan(x => x.End)
                .WithMessage("Start time must be before end time.");
        }
    }
}
