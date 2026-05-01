using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> GetAllAppointmentCancelled(int id)
        {
            return await _dbContext.Appointments
.CountAsync(a =>
    a.PatientId == id &&
    a.AppointmentStatus == Data.Enums.AppointmentStatus.Cancelled);
        }

        public async Task<int> GetAllAppointmentCompleted(int id)
        {
            return await _dbContext.Appointments
           .CountAsync(a =>
               a.PatientId == id &&
               a.AppointmentStatus == Data.Enums.AppointmentStatus.Completed);
        }

        public async Task<int> GetAllAppointmentUpcoming(int id)
        {
            return await _dbContext.Appointments
             .CountAsync(a =>
                 a.PatientId == id &&
                 a.AppointmentStatus == Data.Enums.AppointmentStatus.UPcoming);

        }

        public IQueryable<Appointment> GetAppointmentsByPatientId(int id)
        {
            return _dbContext.Appointments.Where(ww => ww.PatientId == id).Include(ww => ww.Doctor).ThenInclude(ww => ww.User);
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
