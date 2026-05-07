using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Appointments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAppointmentById
{
    public class GetAppointmentByIdQuery:IRequest<Result<AppointmentDtto>>
    {
        public int AppointmentId { get; set; }

        public GetAppointmentByIdQuery(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
