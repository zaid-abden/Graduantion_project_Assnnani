using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Receptionists.Dtos;
using Hangfire.Storage.Monitoring;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetscheduleAppointment
{
    public class GetscheduleAppointmentQuery:IRequest<Result<scheduleAppointmentDto>>
    {
        public int AppointmentId { get; set; }
        public GetscheduleAppointmentQuery(int id)
        {
            this.AppointmentId = id;
        }
    }
}
