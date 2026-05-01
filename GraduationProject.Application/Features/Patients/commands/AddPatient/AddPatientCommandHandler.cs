using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.commands.AddPatient
{
    public class AddPatientCommandHandler : IRequestHandler<AddPatientCommand, Result<int>>
    {
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public AddPatientCommandHandler( IEmailService emailService, IConfiguration configuration,IUnitOfWork unitOfWork,UserManager<User> userManager)
        {
            this.emailService = emailService;
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<Result<int>> Handle(AddPatientCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return Result<int>.Failure(ResultStatus.Failure, "Email already exists");

         
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender.ToString(),
                IsActive = true,
                EmailVerified = false
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return Result<int>.Failure(ResultStatus.Failure,
                    string.Join(", ", result.Errors.Select(e => e.Description)));

          
            await userManager.AddToRoleAsync(user, "Patient");

        
            var patient = new Patient
            {
                UserId = user.Id,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address,
                MedicalHistory = request.MedicalHistory ?? "No previous history",
                Gender = request.Gender,
                CreatedAt = DateTime.Now,
                

            };
            if (request.BloodType.HasValue)
            {
                patient.BloodType = request.BloodType.Value;
            }

            await unitOfWork.Patients.AddAsync(patient);
            await unitOfWork.SaveAsync();

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(
         Encoding.UTF8.GetBytes(token));

            var confirmationLink =
            $"{configuration["AppUrl"]}/api/Authentications/confirm-email?userId={user.Id}&token={encodedToken}";

            var body = $@"
            <h3>Confirm your email</h3>
            <p>Please click the link below:</p>
            <a href='{confirmationLink}'>Confirm Email</a>";


            await emailService.SendEmailAsync(user.Email, "Confirm Email", body);



            return Result<int>.Success(patient.PatientId);

        }
    }
}
