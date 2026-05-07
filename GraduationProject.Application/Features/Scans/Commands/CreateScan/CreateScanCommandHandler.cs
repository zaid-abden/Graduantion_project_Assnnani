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
    public class CreateScanCommandHandler : IRequestHandler<CreateScanCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileServices fileServices;
        private readonly INotificationService notificationService;

        public CreateScanCommandHandler(IUnitOfWork unitOfWork
            ,ICurrentUserService currentUserService
            ,IFileServices fileServices
            ,INotificationService notificationService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileServices = fileServices;
            this.notificationService = notificationService;
        }
        public async Task<Result<string>> Handle(CreateScanCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                ,cancellationToken);
            if (doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");
            var patient = await unitOfWork.Patients.Query()
                .FirstOrDefaultAsync(x => x.PatientId == request.PatientId
                ,cancellationToken);

            if(patient is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Patient profile not found");

     //       var hasVisitedBefore = await unitOfWork.Appointments.Query()
     //.AnyAsync(x =>
     //    x.DoctorId == doctor.DoctorId &&
     //    x.PatientId == patient.PatientId &&
     //    (
     //        x.AppointmentStatus == AppointmentStatus.Completed
     //    ),
     //cancellationToken);
     //       if (!hasVisitedBefore)
     //           return Result<string>.Failure(ResultStatus.Forbidden, "This patient has no previous appointments with the selected doctor.");
            var uploadResult = await fileServices.UploadImageAsync (request.File);
            if(!uploadResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure, uploadResult.Message);

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
      //      await notificationService.SendToUserAsync(
      //doctor.UserId,
      //"Scan Uploaded",
      //"Your scan result is ready",
      //NotificationType.Scan);

            await unitOfWork.Scans.AddAsync(scan);
            await unitOfWork.SaveAsync();
           return Result<string>.Success("Scan uploaded successfully and is pending review.");

        }
    }
}
