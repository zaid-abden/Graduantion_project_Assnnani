using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.AddToQueue
{
    public class AddToQueueCommand:IRequest<Result<string>>
    {
        public int AppointmentId { get; set; }
        public AddToQueueCommand(int id)
        {
            this.AppointmentId = id;
        }
    }
}
