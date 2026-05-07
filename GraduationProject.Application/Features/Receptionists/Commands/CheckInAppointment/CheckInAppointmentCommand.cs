using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.CheckInAppointment
{
    public class CheckInAppointmentCommand : IRequest<Result<string>>
    {
        public int AppointmentId { get; set; }

        public CheckInAppointmentCommand(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
