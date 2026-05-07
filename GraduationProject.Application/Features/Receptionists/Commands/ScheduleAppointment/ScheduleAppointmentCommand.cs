using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.ScheduleAppointment
{
    public class ScheduleAppointmentCommand : IRequest<Result<string>>
    {
        public int PatientId { get; set; }

      
       

        public DateOnly Date { get; set; }
       public int slotId { get; set; }

        public string Reason { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        [JsonIgnore]
      public AppointmentType AppointmentType { get; set; }
        public BookingType BookingType  { get; set; }
    }
}
