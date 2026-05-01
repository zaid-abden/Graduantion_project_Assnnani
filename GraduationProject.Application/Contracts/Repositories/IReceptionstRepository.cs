using GraduationProject.Application.Features.Receptionist.Queries.GetAllPatients;
using GraduationProject.Application.Features.Receptionist.Queries.PatientInfo;
using GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard;
using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IReceptionstRepository : IGenericRepository<Receptionist>
    {
        Task<Receptionist> GetReceptionistbyId(int id);
        Task<List<AppointmentDto>> GetAppointmentToday(int Receptionistid);
        Task<List<QueueDto>> GetQueue(int Receptionistid);
        Task<int> TotalPatient(int ReceptionistId);
        Task<PatientInfoDto> GetPatientInfo(int PaientId);
        Task<List<PatientListDtoForReceptionist>> patientListDtoForReceptionist(int ReceptionistId, int? DoctorId, string? Search, string? status);

        IQueryable<Appointment> GetAllAppointmentForReceptioist(int ReceptionistId);
    }
}
