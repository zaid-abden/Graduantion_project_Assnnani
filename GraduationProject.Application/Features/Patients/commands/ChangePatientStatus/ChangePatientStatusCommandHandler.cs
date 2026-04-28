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

namespace GraduationProject.Application.Features.Patients.commands.ChangePatientStatus
{
    public class ChangePatientStatusCommandHandler : IRequestHandler<ChangePatientStatusCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ChangePatientStatusCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

      

        public async Task<Result<string>> Handle(ChangePatientStatusCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");

            var patient = await unitOfWork.Patients.Query()
     .Where(p =>
         p.PatientId == request.PatientId &&
         (
             p.AssignedDoctorId == doctor.DoctorId ||
             p.Appointments.Any(a =>
                 a.DoctorId == doctor.DoctorId && !a.IsDeleted)
         )
     )
     .FirstOrDefaultAsync(cancellationToken);

            if (patient is null)
                return Result<string>.Failure(ResultStatus.Forbidden,
                    "You are not authorized to access this patient.");

            if (patient.Status == request.Status)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"Patient is already {patient.Status}");
            }

          
            if (patient.Status == PatientStatus.InActive &&
                request.Status == PatientStatus.Pending)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Invalid status transition");
            }

            patient.Status = request.Status;

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Patient status updated successfully");
        }
    }
}
