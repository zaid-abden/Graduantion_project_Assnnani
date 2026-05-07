namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record DoctorStatusDto(
	int DoctorId,
	string FullName,
	string Email,
	string MedicalLicenseNumber,
	int YearsOfExperience,
	int VerificationStatus,
	DateTime? VerifiedAt
);
}
