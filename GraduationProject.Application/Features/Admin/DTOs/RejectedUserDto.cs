namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record RejectedUserDto(
	string Id,
	string FullName,
	string Email,
	string RoleName,
	string RejectionReason,
	DateTime? RejectedAt,
	string? University,           // For Students
	string? MedicalLicenseNumber  // For Doctors
);
}
