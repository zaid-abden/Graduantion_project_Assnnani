using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Receptionists.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientInfo
{
    public class GetPatientInfoQueryHandler
     : IRequestHandler<GetPatientInfoQuery, Result<PatientInfoDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPatientInfoQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PatientInfoDto>> Handle(
            GetPatientInfoQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<PatientInfoDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Unauthorized");

            var userId = currentUserService.UserId;

           
            var doctorId = await unitOfWork.Receptionists.Query()
                .Where(x => x.UserId == userId)
                .Select(x => x.DoctorId)
                .FirstOrDefaultAsync(cancellationToken);

            if (doctorId == 0)
                return Result<PatientInfoDto>.Failure(
                    ResultStatus.NotFound,
                    "Doctor not found");

          
            var patient = await unitOfWork.Patients.Query()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.PatientId == request.Id &&
                    x.AssignedDoctorId == doctorId,
                    cancellationToken);

            if (patient is null)
                return Result<PatientInfoDto>.Failure(
                    ResultStatus.NotFound,
                    "Patient not found or not assigned to this doctor");

          
            var appointment = await unitOfWork.Appointments.Query()
                .Where(x => x.PatientId == request.Id 
                && x.DoctorId == doctorId)
                .Select(x => new PatientInfoDto
                {
                    PatientId = x.PatientId,
                    PatientName = x.Patient.User.FullName,
                    DoctorName = x.Doctor.FullName!,
                    Specialty = x.Doctor.Specialization.Name,
                    Time = x.ScheduleSlot.StartTime,
                    Date = x.ScheduleSlot.Date,
                    Status = x.QueueStatus.ToString(),
                    Amount = (decimal) x.Doctor.price!,
                    AppointmentType = x.AppointmentType.ToString(),
                    PaymentMethod = x.PaymentMethod.ToString(),

                }).FirstOrDefaultAsync(cancellationToken);
           
            if (appointment is null)
                return Result<PatientInfoDto>.Failure(ResultStatus.NotFound, "Patient appointment not found");

            return Result<PatientInfoDto>.Success(appointment);
        }
    }
}
