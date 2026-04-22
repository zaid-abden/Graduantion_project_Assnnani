using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.DeactiveSchedule
{
    public class DeactivateScheduleCommand:IRequest<Result<string>>
    {
        public int ScheduleId { get; set; }
        public DeactivateScheduleCommand(int ScheduleId)
        {
            this.ScheduleId = ScheduleId;
        }
    }
}
