using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Auth.Commands.UnlockUser
{
    public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly ICurrentUserService currentUserService;

        public UnlockUserCommandHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");

          
            if (currentUserService.UserId == request.UserId)
                return Result<string>.Failure(ResultStatus.Failure, "You cannot unlock yourself");

           
            if (!await userManager.IsLockedOutAsync(user))
                return Result<string>.Failure(ResultStatus.Failure, "User is not locked");

           
            var result = await userManager.SetLockoutEndDateAsync(user, null);

            if (!result.Succeeded)
                return Result<string>.Failure(ResultStatus.Failure, "Failed to unlock user");

           
            await userManager.ResetAccessFailedCountAsync(user);

            return Result<string>.Success(user.Id, "User unlocked successfully");
        }
    }
}
