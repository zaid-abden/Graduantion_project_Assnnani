using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Commands.RestoreSchedule
{
    public class RestoreScheduleCommand:IRequest<Result<string>>
    {
        public int ScheduleId { get; set; }
        public RestoreScheduleCommand(int id)
        {
            this.ScheduleId  = id;
        }
    }
}
