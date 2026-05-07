using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Receptionist.Queries.GetAllPatients;
using GraduationProject.Application.Features.Receptionist.Queries.PatientInfo;
//using GraduationProject.Application.Features.Receptionist.Queries.ReceptionistDashboard;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace GraduationProject.Infrastructure.Repositories
{
    public class ReceptionistRepository
        : GenericRepository<Receptionist>, IReceptionstRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReceptionistRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task<List<AppointmentDto>> GetAppointmentToday(int Receptionistid)
        //{
        //    var today = DateOnly.FromDateTime(DateTime.Now);
        //    var result = await (
        //               from s in _dbContext.Receptionists

        //               join d in _dbContext.Doctors
        //                   on s.DoctorId equals d.DoctorId

        //               where s.ReceptionistId == Receptionistid   // 🔥 هنا الفلترة

        //               join a in _dbContext.Appointments
        //                   on d.DoctorId equals a.DoctorId

        //               join sc in _dbContext.ScheduleSlots
        //               .Include(x => x.DoctorSchedule)
        //                   on a.ScheduleSlotId equals sc.Id
        //               where sc.DoctorSchedule.Date == DateOnly.FromDateTime(DateTime.Now)

        //               join pa in _dbContext.Patients
        //               on a.PatientId equals pa.PatientId

        //               select new AppointmentDto
        //               {
        //                   AppointmentId = a.AppointmentId,
        //                   DoctorName = d.User.FullName,
        //                   Specialization = d.Specialization.Name,
        //                   Status = a.AppointmentStatus.ToString(),
        //                   Time = sc.StartTime
        //               }
        //      ).ToListAsync();

        //    return result;

        //}

        //public async Task<List<QueueDto>> GetQueue(int Receptionistid)
        //{
        //    var today = DateOnly.FromDateTime(DateTime.Now);
        //    var result = await (
        //               from s in _dbContext.Receptionists
        //               join d in _dbContext.Doctors
        //                   on s.DoctorId equals d.DoctorId

        //               where s.ReceptionistId == Receptionistid   // 🔥 هنا الفلترة

        //               join a in _dbContext.Appointments
        //                   on d.DoctorId equals a.DoctorId
        //               where a.PatientStatus == PatientStatus.InProgress
        //               || a.PatientStatus == PatientStatus.Waiting
        //               || a.PatientStatus == PatientStatus.CheckedIn

        //               join sc in _dbContext.ScheduleSlots
        //               .Include(x => x.DoctorSchedule)
        //                   on a.ScheduleSlotId equals sc.Id
        //               where sc.DoctorSchedule.Date == DateOnly.FromDateTime(DateTime.Now)

        //               join pa in _dbContext.Patients
        //               on a.PatientId equals pa.PatientId

        //               select new QueueDto
        //               {
        //                   AppointmentId = a.AppointmentId,
        //                   DoctorName = d.User.FullName,
        //                   PatientName = pa.User.FullName,
        //                   Status = a.PatientStatus.ToString(),
        //                   ArrivalTime = a.ArrivelTime
        //               }
        //      ).ToListAsync();

        //    return result;
        //}

        //public async Task<int> TotalPatient(int ReceptionistId)
        //{
        //    var result = await (
        //             from s in _dbContext.Receptionists
        //             join d in _dbContext.Doctors
        //                 on s.DoctorId equals d.DoctorId

        //             where s.ReceptionistId == ReceptionistId   // 🔥 هنا الفلترة

        //             join a in _dbContext.Appointments
        //                 on d.DoctorId equals a.DoctorId
        //             select a
        //           ).CountAsync();
        //    return result;
        //}
        //public async Task<Receptionist> GetReceptionistbyId(int id)
        //{
        //    return await _dbContext.Receptionists.Where(ww => ww.ReceptionistId == id).Include(ww => ww.User).SingleOrDefaultAsync();
        //}

        //private async Task<Patient> GetPatient(int id)
        //{
        //    return await _dbContext.Patients.Where(ww => ww.PatientId == id).Include(ww => ww.User).SingleOrDefaultAsync();
        //}
        //private async Task<List<MedicalHistoryDtoo>> medicalHistory(int id)
        //{
        //    var result = await _dbContext.MedicalRecords
        //        .Include(x => x.Appointment)
        //    .Where(m => m.Appointment.PatientId == id)
        //    .Select(m => new MedicalHistoryDtoo
        //    {
        //        Condition = m.Diagnosis,        // Hypertension / Diabetes
        //                                        // أو تعتبرها diagnosis date
        //        Status = m.Notes ?? "Ongoing"   // fallback لو مفيش notes
        //    })
        //    .ToListAsync();
        //    return result;
        //}
        //private async Task<List<AppointmentDtoPatientInfo>> GetAppointmentDtoPatientInfo(int id)
        //{
        //    var result = await _dbContext.Appointments
        //   .Where(a => a.PatientId == id)
        //   .OrderByDescending(a => a.ScheduleSlot.Date) // الأحدث أولاً
        //   .Take(5)
        //   .Select(a => new AppointmentDtoPatientInfo
        //   {
        //       Title = "General Checkup", // أو لو عندك field حقيقي
        //       DoctorName = a.Doctor.FullName,
        //       Date = a.ScheduleSlot.Date,
        //       Status = a.AppointmentStatus.ToString()
        //   })
        //   .ToListAsync();

        //    return result;
        //}
        //public async Task<PatientInfoDto> GetPatientInfo(int PaientId)
        //{
        //    var patient = await GetPatient(PaientId);
        //    var medicalhistory = await medicalHistory(PaientId);
        //    var apphistory = await GetAppointmentDtoPatientInfo(PaientId);
        //    var result = new PatientInfoDto
        //    {
        //        PatientId = patient.PatientId,
        //        FullName = patient.User.FullName,
        //        Address = patient.Address,
        //        Age = GetAge(patient.User.BirthDate.Value),
        //        Email = patient.User.Email,
        //        Gender = patient.User.Gender,
        //        Phone = patient.User.PhoneNumber,
        //        MedicalHistory = medicalhistory,
        //        RecentAppointments = apphistory,
        //    };
        //    return result;
        //}
        //private int GetAge(DateTime birthDate)
        //{
        //    var today = DateTime.Today;
        //    int age = today.Year - birthDate.Year;

        //    if (birthDate.Date > today.AddYears(-age))
        //        age--;

        //    return age;
        //}

        //public async Task<List<PatientListDtoForReceptionist>> patientListDtoForReceptionist(int ReceptionistId, int? DoctorId, string? Search, string? status)
        //{
        //    var doctorsId = await _dbContext.Receptionists
        //                .Where(r => r.ReceptionistId == ReceptionistId)
        //                .Select(r => r.DoctorId)
        //                .ToListAsync();
        //    var query = _dbContext.Appointments
        //               .Where(a => doctorsId.Contains(a.DoctorId))
        //               .Select(a => new { a, a.Patient, a.Patient.User, a.Doctor });

        //    if (DoctorId.HasValue)
        //    {
        //        query = query.Where(x => x.Doctor.DoctorId == DoctorId.Value);
        //    }

        //    if (!string.IsNullOrEmpty(Search))
        //    {
        //        query = query.Where(x =>
        //            x.User.FullName.Contains(Search) ||
        //            x.User.PhoneNumber.Contains(Search));
        //    }
        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        query = status.ToLower() switch
        //        {
        //            "active" => query.Where(x => x.User.IsActive),
        //            "inactive" => query.Where(x => !x.User.IsActive),
        //            "pending" => query.Where(x => x.a.AppointmentStatus == AppointmentStatus.Pending),
        //            _ => query
        //        };
        //    }

        //    var result = await query
        //            .GroupBy(x => x.a.PatientId)
        //            .Select(g => new PatientListDtoForReceptionist
        //            {
        //                PatientId = g.Key,

        //                FullName = g.First().User.FullName,
        //                Phone = g.First().User.PhoneNumber,

        //                Age = DateTime.Today.Year - g.First().Patient.DateOfBirth.Year -
        //                      (g.First().Patient.DateOfBirth >
        //                       DateTime.Today.AddYears(-(DateTime.Today.Year - g.First().Patient.DateOfBirth.Year)) ? 1 : 0),

        //                Gender = g.First().User.Gender,

        //                Status = g.First().a.AppointmentStatus == AppointmentStatus.Pending ? "Pending" :
        //                                             g.First().User.IsActive ? "Active" : "Inactive",



        //                LastVisit = g.Max(x => x.a.ScheduleSlot.Date),

        //                DoctorName = g
        //                    .OrderByDescending(x => x.a.ScheduleSlot.Date)
        //                    .Select(x => x.Doctor.FullName)
        //                    .FirstOrDefault()
        //            })
        //            .ToListAsync();

        //    return result;

        //}

        //public IQueryable<Appointment> GetAllAppointmentForReceptioist(int ReceptionistId)
        //{
        //    return _dbContext.Appointments
        //        .Where(a => a.Doctor.Receptionist.ReceptionistId
        //                    == ReceptionistId)
        //        .Include(a => a.Patient)
        //        .Include(a => a.ScheduleSlot)
        //        .Include(a => a.Doctor);



        //}
    }
}
