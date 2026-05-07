using GraduationProject.Application.Features.Admin.DTOs;
using GraduationProject.Application.Features.Admin.Queries.GetAllUsers;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
	public interface IAdminRepository : IGenericRepository<Admin>
	{
		Task<List<doctor>> GetPendingDoctorsOnlyAsync();

		Task<doctor?> GetPendingDoctorByIdAsync(int doctorId);

		Task<(List<doctor> Doctors, int TotalCount)> FilterPendingDoctorsAsync(string? searchTerm, int pageNumber, int pageSize);

		Task<string?> ApproveUserAsync(string userId);

		Task<string?> RejectUserAsync(string userId, string reason);

		Task<DashboardStatsDto> GetDashboardStatsAsync();

		Task<List<doctor>> GetRejectedDoctorsOnlyAsync();

		Task<PagedUsersDto> GetAllUsersAsync(GetAllUsersQuery filter);

		Task<User?> GetUserByIdAsync(string id);

		Task<List<doctor>> GetDoctorsByStatusAsync(DoctorVerificationStatus? status);

		Task<List<doctor>> GetDoctorsOnlyAsync();

		Task<List<PatientListDto>> GetPatientsOnlyAsync();

		Task<List<StudentListDto>> GetStudentsOnlyAsync();

		Task<List<ReceptionistListDto>> GetReceptionistsOnlyAsync();

		Task<List<string>> GetEmailsByTargetAsync(string? userId, string? roleName, List<string>? userIds);

		Task<bool> ToggleUserStatusAsync(string userId);
	}
}
