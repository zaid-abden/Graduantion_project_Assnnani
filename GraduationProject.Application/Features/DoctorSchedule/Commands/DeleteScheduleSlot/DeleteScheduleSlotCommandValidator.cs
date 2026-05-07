using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeleteScheduleSlot
{
    public class DeleteScheduleSlotCommandValidator
        : AbstractValidator<DeleteScheduleSlotCommand>
    {
        public DeleteScheduleSlotCommandValidator()
        {
            RuleFor(x => x.SlotId)
                .GreaterThan(0)
                .WithMessage("SlotId must be greater than 0.");
        }
    }
}
