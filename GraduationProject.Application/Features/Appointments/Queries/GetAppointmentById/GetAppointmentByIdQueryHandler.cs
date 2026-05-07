using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Appointments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAppointmentById
{
    public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, Result<AppointmentDtto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAppointmentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<AppointmentDtto>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            var appointment = await unitOfWork.Appointments.Query()
                .Where(x => x.AppointmentId == request.AppointmentId)
                .Select(i => new AppointmentDtto
                {
                    AppointmentId = i.AppointmentId,
                    DoctorName = i.Doctor.User.FullName,
                    PatientName = i.Patient.User.FullName,
                    StartTime = i.ScheduleSlot.StartTime,
                    EndTime = i.ScheduleSlot.EndTime,
                    Status = i.AppointmentStatus.ToString(),
                })
                .FirstOrDefaultAsync(cancellationToken);

            if(appointment is null)

                return Result<AppointmentDtto>.Failure(
       ResultStatus.NotFound,
       $"Appointment with ID {request.AppointmentId} does not exist.");
            return Result<AppointmentDtto>.Success(appointment);


           

        }
    }
}
