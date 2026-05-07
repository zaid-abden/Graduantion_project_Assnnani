using GraduationProject.Application.Common.Results;
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

namespace GraduationProject.Application.Features.StudentDoctors.Commands.CompleteStudentDoctorProfile
{
    public class CompleteStudentDoctorProfileCommandHandler
     : IRequestHandler<CompleteStudentDoctorProfileCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileServices fileServices;

        public CompleteStudentDoctorProfileCommandHandler(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork ,
            IFileServices fileServices)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.fileServices = fileServices;
        }

        public async Task<Result<string>> Handle(CompleteStudentDoctorProfileCommand request, CancellationToken cancellationToken)
        {
           
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");

           
            if (!user.EmailVerified)
                return Result<string>.Failure(ResultStatus.Failure, "Email not verified");

          
            var doctor = await unitOfWork.Doctors.Query()
                .FirstOrDefaultAsync(d => d.SupervisingNumber == request.SupervisingNumber);

            if (doctor == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Invalid supervising code");

           
            var existingStudent = await unitOfWork.StudentDoctors.Query()
                .FirstOrDefaultAsync(s => s.UserId == user.Id);

            if (existingStudent != null)
                return Result<string>.Failure(ResultStatus.Conflict, "Profile already completed");

            var uploadResult = await fileServices.UploadImageAsync(request.File);
         if(!uploadResult.IsSuccess)
                return Result<string>.Failure(ResultStatus.Failure, uploadResult.Error);
            var studentDoctor = new StudentDoctor
            {
                UserId = user.Id,
                DoctorId = doctor.DoctorId,

                NationalId = request.NationalId,
                YearsOfStudy = request.YearsOfStudy
               
            };

            await unitOfWork.StudentDoctors.AddAsync(studentDoctor);
            await unitOfWork.SaveAsync();

            return Result<string>.Success("Student Doctor profile completed successfully");
        }
    }
}
