using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Admin.Queries.GetUserById
{
	public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
	{
		private readonly IAdminRepository _adminRepository;
		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public GetUserByIdHandler(
			IAdminRepository adminRepository,
			UserManager<User> userManager,
			IHttpContextAccessor httpContextAccessor)
		{
			_adminRepository = adminRepository;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _adminRepository.GetUserByIdAsync(request.UserId);

			if (user == null)
				return Result<UserDto>.Failure(ResultStatus.NotFound, "User not found.");

			// 1. Get User Role
			var roles = await _userManager.GetRolesAsync(user);
			var userRole = roles.FirstOrDefault() ?? "No Role Assigned";

			// 2. Construct Full Image URL
			var httpRequest = _httpContextAccessor.HttpContext.Request;
			var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";
			var fullPath = !string.IsNullOrEmpty(user.ImageUrl)
				? $"{baseUrl}/{user.ImageUrl.TrimStart('/')}"
				: $"{baseUrl}/images/default-user.png";

			// 3. Manual Mapping
			var userDto = new UserDto(
				user.Id,
				$"{user.FirstName} {user.LastName}",
				user.Email ?? "",
				user.PhoneNumber,
				userRole,
				fullPath,
				user.Gender,
				user.BirthDate,
				user.IsActive,
				user.CreatedAt
			);

			return Result<UserDto>.Success(userDto, message: "User data retrieved successfully.");
		}
	}
}