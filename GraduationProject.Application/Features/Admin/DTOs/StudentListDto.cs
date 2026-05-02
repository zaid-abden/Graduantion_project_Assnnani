namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record StudentListDto(string Id, string FullName, string Email, string University, int YearOfStudy, bool IsActive);
}
