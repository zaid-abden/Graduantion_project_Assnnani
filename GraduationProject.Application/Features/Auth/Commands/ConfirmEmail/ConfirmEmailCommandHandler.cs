using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result<string>>
    {
        private readonly UserManager<User> userManager;

        public ConfirmEmailCommandHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");
            var decodedToken = Encoding.UTF8.GetString(
            WebEncoders.Base64UrlDecode(request.Token));
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return Result<string>.Failure(ResultStatus.Failure, "Invalid or expired token.");

            return Result<string>.Success("Email confirmed successfully.");

        }
    }
}
