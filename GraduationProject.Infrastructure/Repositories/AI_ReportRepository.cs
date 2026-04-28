using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{
    public class AI_ReportRepository : GenericRepository<AI_Report>, IAI_ReportRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AI_ReportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> labResultsCount(int patientId)
        {
            return await _dbContext.AI_Reports.CountAsync(ww => ww.PredictionResult != null);
        }
    }

}
