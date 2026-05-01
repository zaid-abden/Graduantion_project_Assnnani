using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GraduationProject.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CancelAppointmentCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User is not authenticated");
            var userId = currentUserService.UserId;
            var patient = await unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(c => c.UserId == userId,cancellationToken);
            if(patient is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Patient profile not found");
            var appointment = await unitOfWork.Appointments.Query()
                .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId
                && x.PatientId == patient.PatientId
                ,cancellationToken);
            if(appointment is null)
                return Result<string>.Failure(ResultStatus.NotFound, "No appointment was found with the provided ID for this patient.");
            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled)
                return Result<string>.Failure(
       ResultStatus.Conflict,
       "The appointment is already cancelled and cannot be modified.");
          
            if (appointment.AppointmentStatus == AppointmentStatus.Completed)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Cannot cancel a completed appointment."
                );
            }
            var scheduleSlots = await unitOfWork.ScheduleSlots.Query()
                .FirstOrDefaultAsync(x => x.Id == appointment.ScheduleSlotId
                , cancellationToken);

            var slotDateTime = scheduleSlots.Date.ToDateTime(scheduleSlots.StartTime);
            if (scheduleSlots is null)
                return Result<string>.Failure(
        ResultStatus.NotFound,
        "Schedule slot not found.");

            if (appointment.AppointmentStatus == AppointmentStatus.Confirmed)
            {
                if (slotDateTime <= DateTime.Now.AddHours(2))
                {
                    return Result<string>.Failure(
                        ResultStatus.Conflict,
                        "You cannot cancel a confirmed appointment within 2 hours of its scheduled time."
                    );
                }
            }
            appointment.AppointmentStatus = AppointmentStatus.Cancelled;

            scheduleSlots.Status = SlotStatus.Available;
            await unitOfWork.SaveAsync();
            return Result<string>.Success("The appointment is  cancelled  successfully");
    
        }
    }
}
