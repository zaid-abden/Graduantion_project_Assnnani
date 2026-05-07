using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommand : IRequest<Result<string>>
    {
        public int AppointmentId { get; set; }
        public DateOnly NewDate { get; set; }
        public int NewSlotId { get; set; }
    }
}
