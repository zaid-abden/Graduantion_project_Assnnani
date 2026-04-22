using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.AddAppointment
{
    public class AddAppointmentCommand : IRequest<Result<int>>
    {
        public int ScheduleSlotId { get; set; }
        public string? Notes { get; set; }



        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;


    }
}
