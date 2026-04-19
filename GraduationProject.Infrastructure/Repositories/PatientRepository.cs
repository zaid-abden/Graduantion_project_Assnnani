using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;

namespace GraduationProject.Infrastructure.Repositories
{
	public class PatientRepository
		 : GenericRepository<Patient>, IPatientRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public PatientRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
}
