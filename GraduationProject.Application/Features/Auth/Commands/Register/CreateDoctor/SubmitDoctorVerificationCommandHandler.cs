using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctor
{
    public class SubmitDoctorVerificationCommandHandler : IRequestHandler<SubmitDoctorVerificationCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileServices fileServices;
        private readonly UserManager<User> userManager;
        //doctor-id
      
        public SubmitDoctorVerificationCommandHandler(IUnitOfWork unitOfWork, IFileServices fileServices, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.fileServices = fileServices;
            this.userManager = userManager;
            
        }
        public async Task<Result<string>> Handle(SubmitDoctorVerificationCommand request, CancellationToken cancellationToken)
        {
            //var docId = httpContextAccessor.HttpContext.Request.Headers["doctor-id"].ToString();
            var doctor = await unitOfWork.Doctors.GetByIdAsync(request.DoctorId);
            if (doctor == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor not found in the system.");
            var user = await userManager.FindByIdAsync(doctor.UserId);
            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Associated user details not found.");
            if (!user.EmailVerified)
                return Result<string>.Failure(ResultStatus.Failure, "You must verify your email before submitting verification.");
            doctor.ClinicPhoneNumber = request.ClinicPhone;
            doctor.VerificationStatus = DoctorVerificationStatus.Pending;
            doctor.Street = request.ClinicAddress;
            doctor.SpecializationId = request.SpecializationId;
            doctor.MedicalLicenseNumber = request.MedicalLicenseNumber;
            doctor.YearsOfExperience = request.YearsOfExperience;
            doctor.ClinicName = request.ClinicName;
            doctor.Details = "";
            doctor.About = "";
           doctor.FullName=user.FullName;

            var certResult = await SaveCertificateAsync(request.Certificate);
            if (!certResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure, certResult.Error);

            doctor.DoctorCertificate = certResult.Value;
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Your verification documents have been reviewed by the admin. We will get back to you within 24 hours.");



        }
        private async Task<Result<string>> SaveCertificateAsync(IFormFile certificate)
        {
            if (certificate == null || certificate.Length == 0)
                return Result<string>.Failure(ResultStatus.Failure, "Certificate file is required and cannot be empty.");

            var allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf" };
            var ext = Path.GetExtension(certificate.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(ext))
                return Result<string>.Failure(ResultStatus.Failure,
                    $"File extension '{ext}' is not allowed. Allowed: {string.Join(", ", allowedExtensions)}");

            var maxSizeInMB = 5;
            if (certificate.Length > maxSizeInMB * 1024 * 1024)
                return Result<string>.Failure(ResultStatus.Failure, $"File size exceeds {maxSizeInMB} MB.");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var savePath = Path.Combine("wwwroot/uploads/certificates", fileName);

            
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

          
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await certificate.CopyToAsync(stream);
            }

            
            return Result<string>.Success($"/uploads/certificates/{fileName}");
        }

    }


}
