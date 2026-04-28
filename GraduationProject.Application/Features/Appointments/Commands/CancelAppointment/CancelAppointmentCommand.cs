using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommand : IRequest<Result<string>>
    {
        public int AppointmentId { get; set; }

        public CancelAppointmentCommand(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
