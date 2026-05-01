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

namespace GraduationProject.Application.Features.ApplicationUsers.Commands.DeleteMyAccount
{
    public class DeleteMyAccountCommandHandler : IRequestHandler<DeleteMyAccountCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly ICurrentUserService currentUserService;

        public DeleteMyAccountCommandHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(DeleteMyAccountCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await userManager.FindByIdAsync(currentUserService.UserId!);
            if (currentUser == null)
                return Result<string>.Failure(
        ResultStatus.Unauthorized,
        "The current authenticated user could not be found. Please log in again.");

            currentUser.IsDeleted = true;
            var result = await userManager.UpdateAsync(currentUser);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
              ResultStatus.Failure,
              "Failed to delete your account. Please try again later."
          );
            }

            return Result<string>.Success(currentUser.Id);
        }
    }
}
