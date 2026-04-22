using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
	public interface IDoctorRepository : IGenericRepository<doctor>
	{
		//Task<doctor> GetCurrentDoctor(string userId);
		//IQueryable<doctor> GetAll();

		//public Task<DoctorStatisticsDto> GetDoctorStatsAsync(int doctorId);

		//public Task<DoctorTodaySummaryDto> GetTodaySummaryAsync(int doctorId);

		//public Task<DoctorProfileDto> GetProfileAsync(int doctorId);

		//public Task<bool> UpdateProfileAsync(doctor doctor);

		//public Task<doctor> GetDoctorWithUserAsync(int doctorId);

		public Task<bool> SaveChangesAsync();

	}
}
