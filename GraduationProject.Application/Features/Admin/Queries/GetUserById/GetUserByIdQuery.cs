using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetUserById
{
	public record GetUserByIdQuery(string UserId) : IRequest<Result<UserDto>>;

	public record UserDto(
		string Id,
		string FullName,
		string Email,
		string? PhoneNumber,
		string Role,
		string FullImageUrl,
		string? Gender,
		DateTime? BirthDate,
		bool IsActive,
		DateTime CreatedAt
	);
}
