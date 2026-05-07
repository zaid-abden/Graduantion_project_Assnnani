namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record UserDto(
	string Id,
	string FirstName,
	string LastName,
	string Email,
	string? PhoneNumber,
	string FullImageUrl, // المسار الكامل للصورة
	string? Gender,
	DateTime? BirthDate,
	bool IsActive,
	string Role // الدور الخاص بالمستخدم
);
}
