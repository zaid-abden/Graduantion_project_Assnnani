namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record PendingUserDto(
	string Id,
	string FullName,
	string Email,
	string RoleName,
	string? University,           // For Students
	int? YearOfStudy,             // For Students
	string? MedicalLicenseNumber, // For Doctors
	string? Specialization,       // For Doctors
	string? ImageUrl,             // Profile or License Image
	DateTime? CreatedAt           // Registration Date
);
}
