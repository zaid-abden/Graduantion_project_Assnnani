using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.StudentDoctors.Commands.CreateStudentDoctor
{
    public class CreateStudentDoctorCommandHandler : IRequestHandler<CreateStudentDoctorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileServices fileServices;
        private readonly UserManager<User> userManager;
        private readonly IEmailService emailService;

        public CreateStudentDoctorCommandHandler(IUnitOfWork unitOfWork
            , IFileServices fileServices
            , UserManager<User> userManager
            ,IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            this.fileServices = fileServices;
            this.userManager = userManager;
            this.emailService = emailService;
        }
        public async Task<Result<string>> Handle(CreateStudentDoctorCommand request, CancellationToken cancellationToken)
        {
            var existing = await userManager.FindByEmailAsync(request.Email);
            if (existing != null)
                return Result<string>.Failure(ResultStatus.Conflict, "Email already used");
         
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailVerified = false,
                IsActive = true,
               
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.ValidationError, errors);
            }
            await userManager.AddToRoleAsync(user, "Patient");
            var code = new Random().Next(100000, 999999).ToString();

            var emailVerification = new EmailVerification
            {
                UserId = user.Id,
                Code = code,
                ExpireAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false
            };
           
            await unitOfWork.EmailVerificationRepository.AddVerification(emailVerification);

            await unitOfWork.SaveAsync();
            await emailService.SendEmailAsync(
           user.Email,
           "Verify Your Email",
           $"Your verification code is: {code}"
       );

            return Result<string>.Success("Account created. Please verify your email.");
        }

    }
}
