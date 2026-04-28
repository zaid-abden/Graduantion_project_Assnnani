using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecordAttachment
{
    public class AddMedicalRecordAttachmentCommandHandler : IRequestHandler<AddMedicalRecordAttachmentCommand, Result<int>>
    {
        private readonly IFileServices fileServices;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public AddMedicalRecordAttachmentCommandHandler(IFileServices fileServices, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.fileServices = fileServices;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(AddMedicalRecordAttachmentCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId
                , cancellationToken);
            if (doctor is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Doctor profile not found");
            var medicalRecord = await unitOfWork.MedicalRecords.Query()
                .FirstOrDefaultAsync(c => c.RecordId == request.MedicalRecordId
                && c.DoctorId == doctor.DoctorId
                && !c.IsDeleted
                , cancellationToken);

            if (medicalRecord is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Medical record not found");

            var uploadResult = await fileServices.UploadImageAsync(request.File);
            if (!uploadResult.IsSuccess)
                return Result<int>.Failure(ResultStatus.Failure, "File upload failed: " + uploadResult.Error);
            var attachment = new MedicalRecordAttachment
            {
                MedicalRecordId = request.MedicalRecordId,
                FileName = uploadResult.Value!.FileName,
                FilePath = uploadResult.Value.FileUrl,

            };

            await unitOfWork.MedicalRecordAttachments.AddAsync(attachment);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(attachment.Id, "Attachment added successfully");

        }
    }
}
