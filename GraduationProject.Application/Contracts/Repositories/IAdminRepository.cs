using GraduationProject.Application.Features.Admin.DTOs;
using GraduationProject.Application.Features.Admin.Queries.GetAllUsers;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
	public interface IAdminRepository : IGenericRepository<Admin>
	{
		Task<List<PendingUserDto>> GetPendingUsersAsync();

		Task<string?> ApproveUserAsync(string userId);

		Task<string?> RejectUserAsync(string userId, string reason);

		Task<DashboardStatsDto> GetDashboardStatsAsync();

		Task<List<RejectedUserDto>> GetRejectedUsersAsync();

		Task<PagedUsersDto> GetAllUsersAsync(GetAllUsersQuery filter);

		Task<List<DoctorStatusDto>> GetDoctorsByStatusAsync(DoctorVerificationStatus? status);

		Task<List<DoctorListDto>> GetDoctorsOnlyAsync();

		Task<List<PatientListDto>> GetPatientsOnlyAsync();

		Task<List<StudentListDto>> GetStudentsOnlyAsync();

		Task<List<ReceptionistListDto>> GetReceptionistsOnlyAsync();

		Task<List<string>> GetEmailsByTargetAsync(string? userId, string? roleName, List<string>? userIds);

		Task<bool> ToggleUserStatusAsync(string userId);
	}
}
