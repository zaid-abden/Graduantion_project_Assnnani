using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{

    public class DoctorRepository
        : GenericRepository<doctor>, IDoctorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DoctorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        //public IQueryable<doctor> GetAll()
        //{
        //    return _dbContext.Doctors.AsQueryable().Include(ww => ww.User);
        //}

        //public async Task<doctor> GetCurrentDoctor(string userId)
        //{
        //    var doctor = await _dbContext.Doctors.Include(x => x.User)
        //        .FirstOrDefaultAsync(d => d.UserId == userId);
        //    return doctor!;
        //}

        //public async Task<DoctorStatisticsDto> GetDoctorStatsAsync(int doctorId)
        //{
        //    // حساب المواعيد
        //    var totalAppointments = await _dbContext.Appointments
        //        .CountAsync(a => a.DoctorSchedule.DoctorId == doctorId);

        //    // حساب المرضى المميزين الذين زاروا هذا الدكتور
        //    var totalPatients = await _dbContext.Appointments
        //        .Where(a => a.DoctorSchedule.DoctorId == doctorId)
        //        .Select(a => a.PatientId)
        //        .Distinct()
        //        .CountAsync();

        //    // حساب تقارير الأشعة (Scan Reviews) من جدول AI_Reports
        //    var scanReviews = await _dbContext.AI_Reports
        //        .CountAsync(r => r.DoctorId == doctorId);

        //    // حساب نسبة الرضا من جدول Feedbacks
        //    var satisfaction = await _dbContext.Feedbacks
        //        .Where(f => f.DoctorId == doctorId)
        //        .Select(f => (double?)f.Rating)
        //        .AverageAsync() ?? 0.0;

        //    return new DoctorStatisticsDto
        //    {
        //        TotalAppointments = totalAppointments,
        //        TotalPatients = totalPatients,
        //        ScanReviews = scanReviews,
        //        Satisfaction = (int)Math.Round(satisfaction, 1) // تقريب لرقم عشري واحد
        //    };
        //}

        //public async Task<DoctorTodaySummaryDto> GetTodaySummaryAsync(int doctorId)
        //{
        //    var today = DateTime.Today;

        //    var appointments = await _dbContext.Appointments
        //        .Where(a => a.DoctorSchedule.DoctorId == doctorId &&
        //                    a.AppointmentTime.Date == today)
        //        .Select(a => new AppointmentSummaryDto
        //        {
        //            AppointmentId = a.AppointmentId,
        //            // افترضت أن جدول الـ Patient مرتبط بجدول الـ AspNetUsers لجلب الاسم
        //            PatientName = a.Patient.User.FirstName + " " + a.Patient.User.LastName,
        //            AppointmentTime = a.AppointmentTime,
        //            BookingType = a.BookingType.ToString()
        //        })
        //        .OrderBy(a => a.AppointmentTime)
        //        .ToListAsync();

        //    return new DoctorTodaySummaryDto
        //    {
        //        TotalTodayAppointments = appointments.Count,
        //        Appointments = appointments
        //    };
        //}

        //public async Task<DoctorProfileDto> GetProfileAsync(int doctorId)
        //{
        //    return await _dbContext.Doctors
        //        .Where(d => d.DoctorId == doctorId)
        //        .Select(d => new DoctorProfileDto
        //        {
        //            FullName = d.User.FirstName + " " + d.User.LastName,
        //            Email = d.User.Email,
        //            PhoneNumber = d.User.PhoneNumber,
        //            ImageUrl = d.ImageUrl,
        //            About = d.About,
        //            YearsOfExperience = d.YearsOfExperience,
        //            FullAddress = $"{d.Country}, {d.City}, {d.Street}",
        //            Degree = d.Degree,
        //            Rating = d.Rating
        //        })
        //        .FirstOrDefaultAsync();
        //}

        public async Task<bool> UpdateProfileAsync(doctor doctor)
        {
            // Since the object is already tracked by EF (from the Get method), 
            // calling Update marks it as modified.
            Update(doctor);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<doctor> GetDoctorWithUserAsync(int doctorId)
        {
            return await Query()
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
