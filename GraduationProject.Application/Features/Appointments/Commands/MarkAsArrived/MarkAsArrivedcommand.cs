using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Appointments.Commands.MarkAsArrived
{
    public class MarkAsArrivedCommand : IRequest<Result<string>>
    {
        public int AppointmentId { get; set; }

        public MarkAsArrivedCommand(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
