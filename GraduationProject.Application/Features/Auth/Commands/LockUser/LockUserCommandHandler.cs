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

namespace GraduationProject.Application.Features.Auth.Commands.LockUser
{
    public class LockUserCommandHandler : IRequestHandler<LockUserCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly ICurrentUserService currentUserService;

        public LockUserCommandHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(LockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                
            return Result<string>.Failure(ResultStatus.NotFound, "User not found");
            if (!user.LockoutEnabled)
            {
                user.LockoutEnabled = true;
                await userManager.UpdateAsync(user);
            }
            var result = await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            if (!result.Succeeded)
                return Result<string>.Failure(ResultStatus.Failure, "Failed to lock user");


            return Result<string>.Success(user.Id, "User locked successfully");
        }
    }
}
