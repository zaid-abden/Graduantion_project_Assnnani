using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.ApplicationUsers.Commands.UpdateImageProfile
{
    public class UpdateImageProfileCommandHandler : IRequestHandler<UpdateImageProfileCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;
        private readonly IFileServices fileService;
        private readonly ICurrentUserService currentUserService;

        public UpdateImageProfileCommandHandler(UserManager<User> userManager, IFileServices fileService, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.fileService = fileService;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(UpdateImageProfileCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User not authenticated.");

            var userId = currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<string>.Failure(ResultStatus.Unauthorized, "User ID not found.");

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found.");

            if (user.ImageUrl is not null)
            {
                var resRemove = fileService.Remove(user.ImageUrl);
                if (!resRemove.IsSuccess)
                    return Result<string>.Failure(ResultStatus.Failure, resRemove.Error);
            }

            var res = await fileService.UploadImageAsync(request.ProfileImage!);
            if (!res.IsSuccess)
                return Result<string>.Failure(res.Status, res.Error);
            user.ImageUrl = res.Value!.FileUrl;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
              ResultStatus.Failure,
              "Failed to add user's ProfileImage. Please try again later."
          );


            }

            return Result<string>.Success(user.Id);
        }
    }
}
