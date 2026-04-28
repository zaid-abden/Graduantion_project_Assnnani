using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.CreateScan
{
    public class CreateScanCommandHandler : IRequestHandler<CreateScanCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileServices fileServices;

        public CreateScanCommandHandler(IUnitOfWork unitOfWork
            ,ICurrentUserService currentUserService
            ,IFileServices fileServices)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileServices = fileServices;
        }
        public async Task<Result<int>> Handle(CreateScanCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                ,cancellationToken);
            if (doctor is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Doctor profile not found");
            var patient = await unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId
                ,cancellationToken);

            if(patient is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Patient profile not found");

            var hasVisitedBefore = await unitOfWork.Appointments.Query()
     .AnyAsync(x =>
         x.DoctorId == doctor.DoctorId &&
         x.PatientId == patient.PatientId &&
         (
             x.AppointmentStatus == AppointmentStatus.Completed ||
             x.AppointmentStatus == AppointmentStatus.arrived
         ),
     cancellationToken);
            if (!hasVisitedBefore)
                return Result<int>.Failure(ResultStatus.Forbidden, "This patient has no previous appointments with the selected doctor.");
            var uploadResult = await fileServices.UploadImageAsync (request.File);
            if(!uploadResult.IsSuccess)
                return Result<int>.Failure(ResultStatus.Failure, uploadResult.Message);

            var scan = new Scan
            {
                DoctorId = doctor.DoctorId,
                PatientId = patient.PatientId,
                Status = ScanStatus.Pending,
                FileUrl = uploadResult.Value!.FileUrl,
                UploadedAt = DateTime.Now,
              ScanType = request.ScanType,
               CreatedAt = DateTime.Now,
              
               Priority = request.Priority,
            FileName = uploadResult.Value.FileName,
            FileSize = uploadResult.Value.FileSize,
            FileType = uploadResult.Value.FileType,
         AIStatus = AIStatus.Pending ,
     
              

            };

            await unitOfWork.Scans.AddAsync(scan);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(scan.Id);

        }
    }
}
