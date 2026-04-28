using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IMedicalRecordRepository : IGenericRepository<medicalRecord>
    {
        Task<int> prescriptionsCount(int patientId);
        Task<int> recordsCount(int patientId);
    }
}
