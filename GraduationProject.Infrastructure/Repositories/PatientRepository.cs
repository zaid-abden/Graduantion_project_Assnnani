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

        public async Task<Patient> GetpatientByIdAsync(int id)
        {
            return await _dbContext.Patients.FindAsync(id);
        }
        public async Task UpdateAsync(Patient entity)
        {
            _dbContext.Patients.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

    }
}
