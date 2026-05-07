using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.StartConsultation
{
    public class StartConsultationCommand : IRequest<Result<string>>
    {
        public int AppointmentId { get; set; }

        public StartConsultationCommand(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
