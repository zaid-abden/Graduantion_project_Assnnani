using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Application.Features.Auth.Dtos;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Auth.Commands.Login
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthDto>>
	{
		private readonly UserManager<User> userManager;
		private readonly IAuthService authService;

		public LoginUserCommandHandler(UserManager<User> userManager, IAuthService authService)
		{
			this.userManager = userManager;
			this.authService = authService;
		}
		public async Task<Result<AuthDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var user = await userManager.FindByEmailAsync(request.Email);
			if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
				return Result<AuthDto>.Failure(ResultStatus.Failure, "Invalid email or password.");
			if (await userManager.IsLockedOutAsync(user))
				return Result<AuthDto>.Failure(ResultStatus.Failure, "User is locked");
			if (user.IsDeleted)
				return Result<AuthDto>.Failure(
					ResultStatus.Unauthorized,
					"Invalid email or password.");
			var token = await authService.GenerateToken(user);
			var userRoles = await userManager.GetRolesAsync(user);
			var authDto = new AuthDto
			{
				Token = token,
				ExpiresOn = DateTime.Now.AddMinutes(60),
				Email = request.Email,
				IsAuthenticated = true,
				Roles = userRoles.FirstOrDefault() ?? string.Empty,
				Message = "You have logged in successfully",
				Username = request.Email
			};
			return Result<AuthDto>.Success(authDto);
		}
	}
}
