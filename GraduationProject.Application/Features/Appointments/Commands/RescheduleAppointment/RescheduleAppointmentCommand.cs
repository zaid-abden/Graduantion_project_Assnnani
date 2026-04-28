using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Appointments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommand : IRequest<Result<AddAppointmentResponseDto>>
    {
        public int AppointmentId { get; set; }
        public int NewScheduleSlotId { get; set; }

        public RescheduleAppointmentCommand(int appointmentId, int newScheduleSlotId)
        {
            AppointmentId = appointmentId;
            NewScheduleSlotId = newScheduleSlotId;
        }
    }
}
