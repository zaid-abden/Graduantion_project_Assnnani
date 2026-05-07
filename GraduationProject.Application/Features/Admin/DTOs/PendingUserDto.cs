namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record PendingUserDto(
		string Id,
		string FullName,
		string Email,
		string RoleName,
		string? Gender,               // إضافة النوع
		string? University,
		int? YearOfStudy,
		string? MedicalLicenseNumber,
		string? ProfileImageUrl,
		string? LicenseCardUrl,
		DateTime? CreatedAt
	);
}
