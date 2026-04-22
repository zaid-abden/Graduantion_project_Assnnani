using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Features.ApplicationUsers.Dtos;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.ApplicationUsers.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<ApplicationUserDto>>
    {
        private readonly UserManager<User> userManager;
        private readonly ICurrentUserService currentUserService;

        public GetUserProfileQueryHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<ApplicationUserDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<ApplicationUserDto>.Failure(ResultStatus.Unauthorized, "You are not authorized to access this resource.");


            var user = await userManager.FindByIdAsync(currentUserService.UserId!);


            if (user == null)
                return Result<ApplicationUserDto>.Failure(
                 ResultStatus.Unauthorized,
                   "The current authenticated user could not be found. Please log in again.");
            var userDto = new ApplicationUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
            
                Roles = await userManager.GetRolesAsync(user)
            };

            return Result<ApplicationUserDto>.Success(userDto);
        }
    }
}
