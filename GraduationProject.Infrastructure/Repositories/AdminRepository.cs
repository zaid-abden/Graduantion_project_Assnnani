using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;

namespace GraduationProject.Infrastructure.Repositories
{
	public class AdminRepository
		 : GenericRepository<Admin>, IAdminRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public AdminRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
