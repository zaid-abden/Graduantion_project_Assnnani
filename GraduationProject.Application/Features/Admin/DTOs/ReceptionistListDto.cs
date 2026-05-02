namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record ReceptionistListDto(string Id, string FullName, string Email, string DoctorName, string Shift, bool IsActive);
}
