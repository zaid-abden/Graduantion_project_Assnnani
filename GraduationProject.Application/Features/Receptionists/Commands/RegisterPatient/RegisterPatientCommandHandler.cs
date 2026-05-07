using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Application.Features.Patients.commands.AddPatient;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Commands.RegisterPatient
{
    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, Result<string>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;
        private readonly INotificationService _notificationService;

        public RegisterPatientCommandHandler(ICurrentUserService currentUserService
            , IConfiguration configuration
            , IUnitOfWork unitOfWork
            , UserManager<User> userManager
            ,INotificationService notificationService)
        {
            this.currentUserService = currentUserService;
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this._notificationService = notificationService;
        }
        public async Task<Result<string>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");
            var userId = currentUserService.UserId;
            var doctorId = await unitOfWork.Receptionists.Query()
                .Where(c => c.UserId == userId)
                .Select(c => c.DoctorId)
                .FirstOrDefaultAsync(cancellationToken);
           




            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return Result<string>.Failure(ResultStatus.Failure, "Email already exists");


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
                return Result<string>.Failure(ResultStatus.Failure,
                    string.Join(", ", result.Errors.Select(e => e.Description)));


            await userManager.AddToRoleAsync(user, "Patient");


            var patient = new Patient
            {
                UserId = user.Id,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address,
              
                Gender = request.Gender,
                CreatedAt = DateTime.Now,
                AssignedDoctorId = doctorId,
                BloodType = request.BloodType
               
            };
            if (request.BloodType.HasValue)
            {
                patient.BloodType = request.BloodType.Value;
            }
            patient.Status = PatientStatus.Pending;
            await unitOfWork.Patients.AddAsync(patient);


          

            await unitOfWork.SaveAsync();

            await _notificationService.SendToRoleAsync(
      "Admin",
      "New Patient Registered",
      $"Patient {patient.User.FullName} has been registered.",
      NotificationType.Info
  );


            await _notificationService.SendToUserAsync(
    patient.UserId,
    "Account Created",
    "Your account has been created successfully.",
    NotificationType.Success
);


            return Result<string>.Success("Patient added successfully");

        }
    }
}
