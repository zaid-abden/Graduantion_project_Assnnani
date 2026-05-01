using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {

        Task<Patient> GetpatientByIdAsync(int id);
        Task UpdateAsync(Patient entity);
        IQueryable<Appointment> GetAppointmentsByPatientId(int id);
        Task<int> GetAllAppointmentUpcoming(int id);
        Task<int> GetAllAppointmentCompleted(int id);
        Task<int> GetAllAppointmentCancelled(int id);
    }
}
