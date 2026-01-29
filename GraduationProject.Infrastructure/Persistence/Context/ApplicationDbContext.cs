using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Context
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AI_Report> AI_Reports { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<doctor> Doctors { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<StudentDoctor> StudentDoctors { get; set; }
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ==================== Doctors ====================
            builder.Entity<doctor>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasMany(d => d.MedicalRecords)
                .WithOne(m => m.Doctor)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasMany(d => d.AIReports)
                .WithOne(r => r.Doctor)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasMany(d => d.Feedbacks)
                .WithOne(f => f.Doctor)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasMany(d => d.Verifications)
                .WithOne(v => v.Doctor)
                .HasForeignKey(v => v.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasMany(d => d.Schedules)
                .WithOne(s => s.Doctor)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasOne(d => d.Receptionist)
                .WithOne(r => r.Doctor)
                .HasForeignKey<Receptionist>(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==================== Patients ====================
            builder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                .HasMany(p => p.MedicalRecords)
                .WithOne(m => m.Patient)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                .HasMany(p => p.AIReports)
                .WithOne(r => r.Patient)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                .HasMany(p => p.Feedbacks)
                .WithOne(f => f.Patient)
                .HasForeignKey(f => f.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==================== StudentDoctors ====================
            builder.Entity<StudentDoctor>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

           

            // ==================== Receptionists ====================
            builder.Entity<Receptionist>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==================== Admins ====================
            builder.Entity<Admin>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Admin>()
                .HasMany(a => a.Verifications)
                .WithOne(v => v.Admin)
                .HasForeignKey(v => v.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==================== Verifications ====================
            builder.Entity<Verification>()
                .HasOne(v => v.Doctor)
                .WithMany(d => d.Verifications)
                .HasForeignKey(v => v.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<doctor>()
    .HasOne(d => d.User)
    .WithOne() // أو WithMany() لو User ممكن يكون عنده أكثر من doctor
    .HasForeignKey<doctor>(d => d.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Verification>()
                .HasOne(v => v.Admin)
                .WithMany(a => a.Verifications)
                .HasForeignKey(v => v.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==================== DoctorSchedules ====================
            builder.Entity<DoctorSchedule>()
                .HasMany(ds => ds.Appointments)
                .WithOne(a => a.DoctorSchedule)
                .HasForeignKey(a => a.DoctorScheduleId)
                .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<StudentDoctor>()
    .HasMany(s => s.MedicalRecords)
    .WithOne(m => m.StudentDoctor)
    .HasForeignKey(m => m.StudentDoctorId)
    .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<StudentDoctor>()
                .HasMany(s => s.AIReports)
                .WithOne(r => r.StudentDoctor)
                .HasForeignKey(r => r.StudentDoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<StudentDoctor>()
                .HasMany(s => s.Verifications)
                .WithOne(v => v.StudentDoctor)
                .HasForeignKey(v => v.StudentDoctorId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}

    

