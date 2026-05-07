//using AutoMapper;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationProject.Data.Models;
using GraduationProject.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctor
{
    //public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Result<string>>
    //{
    //    private readonly UserManager<User> userManager;
    //    private readonly IUnitOfWork unitOfWork;

    //    private readonly IEmailService emailService;
    //    private readonly IHttpContextAccessor httpContextAccessor;
    //    public CreateDoctorCommandHandler(
    //        UserManager<User> userManager,
    //        IUnitOfWork unitOfWork,

    //        IEmailService emailService,IHttpContextAccessor httpContextAccessor)
    //    {
    //        this.userManager = userManager;
    //        this.unitOfWork = unitOfWork;

    //        this.emailService = emailService;
    //        this.httpContextAccessor=httpContextAccessor;
    //    }

    //    public async Task<Result<string>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    //    {
    //        var existingUser = await userManager.FindByEmailAsync(request.Email);
    //        if (existingUser != null)
    //            return Result<string>.Failure(ResultStatus.Conflict, "Email is already in use.");
    //        var user = new User
    //        {
    //            FirstName = request.FirstName,
    //            LastName = request.LastName,
    //            Email = request.Email,
    //            UserName = request.Email,
    //            PhoneNumber = request.PhoneNumber,
    //            EmailVerified = false,

    //            IsActive = true
    //        };
    //        var result = await userManager.CreateAsync(user, request.Password);
    //        if (!result.Succeeded)
    //        {
    //            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
    //            return Result<string>.Failure(ResultStatus.ValidationError, errors);
    //        }
    //        await userManager.AddToRoleAsync(user, "Doctor");

    //        var doc = new doctor
    //        {
    //            UserId=user.Id,
    //            YearsOfExperience=0,
    //            VerificationStatus=DoctorVerificationStatus.NotSubmitted,
    //            Country="",
    //            City="",
    //            Street="",
    //            Details="",
    //            ClinicName="",
    //            MedicalLicenseNumber="",

    //        };
    //        await unitOfWork.Doctors.AddAsync(doc);
    //        await unitOfWork.SaveAsync();

    //        var code = new Random().Next(100000, 999999).ToString();
    //        var emailVerification = new EmailVerification
    //        {
    //            UserId = user.Id,
    //            Code = code,
    //            ExpireAt = DateTime.UtcNow.AddMinutes(15),
    //            IsUsed = false
    //        };
    //        await unitOfWork.EmailVerificationRepository.AddVerification(emailVerification);
    //        await emailService.SendEmailAsync(user.Email,"Verify Your Email", $"Your verification code is: {code}");
    //        //httpContextAccessor.HttpContext.Request.Headers.Add("doctor-id", doc.DoctorId.ToString()); 
    //        return Result<string>.Success($"{doc.DoctorId}");
    //    }


    //}
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailService emailService;

        public CreateDoctorCommandHandler(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }

        public async Task<Result<string>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
           
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return Result<string>.Failure(ResultStatus.Conflict, "Email is already in use.");

          
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailVerified = false,
                IsActive = true
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.ValidationError, errors);
            }

            await userManager.AddToRoleAsync(user, "Doctor");

          

            string code;

            do
            {
                code = GenerateCode();
            }
            while (await unitOfWork.Doctors.Query()
                .AnyAsync(d => d.SupervisingNumber == code));

           
            var doc = new doctor
            {
                UserId = user.Id,
                SupervisingNumber = code,

                YearsOfExperience = 0,
                VerificationStatus = DoctorVerificationStatus.NotSubmitted,

                Country = "",
                City = "",
                Street = "",
                Details = "",
                ClinicName = "",
                MedicalLicenseNumber = ""
            };

            await unitOfWork.Doctors.AddAsync(doc);
            await unitOfWork.SaveAsync();

          
            var verificationCode = new Random().Next(100000, 999999).ToString();

            var emailVerification = new EmailVerification
            {
                UserId = user.Id,
                Code = verificationCode,
                ExpireAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false
            };

            await unitOfWork.EmailVerificationRepository.AddVerification(emailVerification);

            await emailService.SendEmailAsync(
                user.Email,
                "Verify Your Email",
                $"Your verification code is: {verificationCode}"
            );

          

            return Result<string>.Success(
                $"DoctorId: {doc.DoctorId}, SupervisingCode: {doc.SupervisingNumber}"
            );
        }

     
        private string GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Range(0, 6)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
    }
}
