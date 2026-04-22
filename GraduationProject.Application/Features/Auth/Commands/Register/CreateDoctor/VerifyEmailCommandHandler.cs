using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctor
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;
        public VerifyEmailCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if(user==null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found.");
            var verification = await unitOfWork.EmailVerificationRepository.GetVerifications();
            var emailVerification = verification.FirstOrDefault(v => v.UserId == user.Id && v.Code == request.Code && !v.IsUsed && v.ExpireAt > DateTime.UtcNow);
            if(emailVerification==null)
                return Result<string>.Failure(ResultStatus.ValidationError, "Invalid or expired verification code.");
           emailVerification.IsUsed = true;
            user.EmailVerified = true;
            await userManager.UpdateAsync(user);
            //await unitOfWork.EmailVerificationRepository.UpdateVerification(emailVerification);
            await unitOfWork.SaveAsync();
            return Result<string>.Success("Email verified successfully.");

        }
    }
}
