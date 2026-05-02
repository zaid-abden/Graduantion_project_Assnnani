namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record UserListItemDto(
	string Id,
	string FullName,
	string Email,
	string? PhoneNumber,
	string Role,
	bool IsActive,
	string? Gender,
	DateTime CreatedAt,
	string? ImageUrl
);

	public record PagedUsersDto(
	List<UserListItemDto> Users,
	int TotalCount
);
}
