using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.RestoreScheduleSlot
{
    public class RestoreScheduleSlotCommand : IRequest<Result<string>>
    {
        public int SlotId { get; set; }
        public RestoreScheduleSlotCommand(int id)
        {
            SlotId = id;
        }
    }
}
