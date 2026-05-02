namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record DoctorListDto(string Id, string FullName, string Email, string Specialization, string LicenseNumber, bool IsActive);
}
