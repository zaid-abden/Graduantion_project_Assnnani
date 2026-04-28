using GraduationProject.Application.Features.Patients.Queries.PatientDashborad;
using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<int> upcomingAppointmentsCount(int appointmentId);
        Task<List<UpcomingAppointmentDto>> upcomingAppointment(int paitenttId);

    }
}
