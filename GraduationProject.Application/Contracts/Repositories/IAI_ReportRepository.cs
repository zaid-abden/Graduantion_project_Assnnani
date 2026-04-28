using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IAI_ReportRepository : IGenericRepository<AI_Report>
    {
        Task<int> labResultsCount(int patientId);
    }
}
