using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentCommand : IRequest<Result<AddAppointmentResponseDto>>
    {
        public int AppointmentId { get; set; }

        public int ScheduleSlotId { get; set; }

        public string? Notes { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
