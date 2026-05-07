using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeleteScheduleSlot
{
    public class DeleteScheduleSlotCommand : IRequest<Result<string>>
    {
        public int SlotId { get; set; }
        public DeleteScheduleSlotCommand(int id)
        {
            SlotId = id;
        }
    }
}
