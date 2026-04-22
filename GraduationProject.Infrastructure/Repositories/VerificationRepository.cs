using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;

namespace GraduationProject.Infrastructure.Repositories
{
	public class VerificationRepository
		: GenericRepository<Verification>, IVerificationRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public VerificationRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
