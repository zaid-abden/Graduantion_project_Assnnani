using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{
    public class MedicalReportRepository : GenericRepository<medicalRecord>, IMedicalRecordRepository
    {
        private readonly ApplicationDbContext dbContext;
        public MedicalReportRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> prescriptionsCount(int patientId)
        {
            return await dbContext.MedicalRecords
                .Include(x => x.Appointment)
                .Where(ww => ww.Appointment.PatientId == patientId).CountAsync();
        }

        public async Task<int> recordsCount(int patientId)
        {
            return await dbContext.MedicalRecords.Include(c => c.Appointment)
                .Where(ww => ww.Appointment.PatientId == patientId).CountAsync();
        }
    }
}
