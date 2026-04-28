using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Patients.Queries.PatientDashborad;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{
    public class AppointmentRepository
        : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UpcomingAppointmentDto>> upcomingAppointment(int paitenttId)
        {
            var resut = await (from a in _dbContext.Appointments
                               where a.PatientId == paitenttId

                               join d in _dbContext.Doctors
                                   on a.DoctorId equals d.DoctorId

                               join p in _dbContext.Users
                                   on d.UserId equals p.Id

                               join s in _dbContext.ScheduleSlots
                                   on a.ScheduleSlotId equals s.Id
                               where s.StartTime > TimeOnly.FromDateTime(DateTime.Now)
                               select new UpcomingAppointmentDto
                               {
                                   DoctorName = p.FullName,
                                   Specialty = d.Specialization.Name,
                                   Date = s.Date,
                                   Starttime = s.StartTime,
                                   Status = s.Status.ToString(),


                               }
                        ).ToListAsync();
            return resut;

        }

        public async Task<int> upcomingAppointmentsCount(int appointmentId)
        {
            return await _dbContext.Appointments.Where(ww => ww.PatientId == appointmentId && ww.CreatedAt >= DateTime.Now).CountAsync();
        }
    }
}
