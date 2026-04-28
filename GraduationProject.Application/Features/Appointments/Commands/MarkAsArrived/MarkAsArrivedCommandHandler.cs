using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Application.Features.Appointments.Commands.MarkAsArrived
{
    public class MarkAsArrivedCommandHandler : IRequestHandler<MarkAsArrivedCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public MarkAsArrivedCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(MarkAsArrivedCommand request, CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.Appointments.Query()
                 .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId
                 && !x.IsDeleted
                 , cancellationToken);
            if (appointment == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Appointment not found");

            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot mark a canceled appointment as arrived");

            if (appointment.AppointmentStatus == AppointmentStatus.arrived)
                return Result<string>.Failure(ResultStatus.Conflict, "Appointment already marked as arrived");

            appointment.AppointmentStatus = AppointmentStatus.arrived;
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Appointment marked as arrived successfully");
        }
    }
}
