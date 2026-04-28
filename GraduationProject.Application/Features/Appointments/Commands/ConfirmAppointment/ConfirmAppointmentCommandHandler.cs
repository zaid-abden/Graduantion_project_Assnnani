using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Commands.ConfirmAppointment
{
    public class ConfirmAppointmentCommandHandler
         : IRequestHandler<ConfirmAppointmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;

        public ConfirmAppointmentCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(
            ConfirmAppointmentCommand request,
            CancellationToken cancellationToken)
        {
          
            var appointment = await unitOfWork.Appointments.Query()
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId,
                    cancellationToken);

            if (appointment is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Appointment not found.");
            }

          
            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Cannot confirm a cancelled appointment.");
            }

            if (appointment.AppointmentStatus == AppointmentStatus.Completed)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Appointment is already completed.");
            }

            if (appointment.AppointmentStatus == AppointmentStatus.Confirmed)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Appointment is already confirmed.");
            }

           
            appointment.AppointmentStatus = AppointmentStatus.Confirmed;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Appointment confirmed successfully.");
        }
    }
}
