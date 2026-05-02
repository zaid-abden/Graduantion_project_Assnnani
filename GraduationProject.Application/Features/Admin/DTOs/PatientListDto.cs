namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record PatientListDto(string Id, string FullName, string Email, string? MedicalHistory, bool IsActive);
}
