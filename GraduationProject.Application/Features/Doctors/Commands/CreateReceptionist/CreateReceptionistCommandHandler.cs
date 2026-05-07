using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateReceptionist
{
    public class CreateReceptionistCommandHandler : IRequestHandler<CreateReceptionistCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<User> userManager;
        private readonly IFileServices fileServices;

        public CreateReceptionistCommandHandler(IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService
            , UserManager<User> userManager
            , IFileServices fileServices)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.userManager = userManager;
            this.fileServices = fileServices;
        }
        public async Task<Result<string>> Handle(CreateReceptionistCommand request, CancellationToken cancellationToken)
        {

            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Unauthorized access. Please log in");

            var userId = currentUserService.UserId;


            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (doctor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Doctor profile not found");


            if (await userManager.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
                return Result<string>.Failure(ResultStatus.Conflict, "Email already exists");

            if (await userManager.Users.AnyAsync(u => u.UserName == request.Username, cancellationToken))
                return Result<string>.Failure(ResultStatus.Conflict, "Username already exists");


            var names = request.FullName
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var firstName = names.FirstOrDefault() ?? "";
            var lastName = names.Length > 1 ? names.Last() : "";


            var user = new User
            {
                Email = request.Email,
                PhoneNumber = request.Phone,
                UserName = request.Username,
                FirstName = firstName,
                LastName = lastName
            };

            var createResult = await userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.ValidationError, errors);
            }


            await userManager.AddToRoleAsync(user, "Receptionist");


            string? imageUrl = null;

            if (request.Image != null)
            {
                var uploadResult = await fileServices.UploadImageAsync(request.Image);

                if (!uploadResult.IsSuccess)
                    return Result<string>.Failure(ResultStatus.Failure, "Image upload failed");

                imageUrl = uploadResult.Value?.FileUrl;
            }


            var receptionist = new Data.Models.Receptionist
            {
                UserId = user.Id,
                DoctorId = doctor.DoctorId,
                Shift = ShiftType.Morning,

            };

            await unitOfWork.Receptionists.AddAsync(receptionist);
            await unitOfWork.SaveAsync();


            return Result<string>.Success("Receptionist created successfully");






        }
    }
}
