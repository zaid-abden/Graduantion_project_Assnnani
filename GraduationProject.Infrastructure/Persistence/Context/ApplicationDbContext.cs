using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public DbSet<ScheduleSlot> ScheduleSlots { get; set; }
        public DbSet<doctor> Doctors { get; set; }
        public DbSet<doctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<medicalRecord> MedicalRecords { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<MedicalRecordAttachment> MedicalRecordAttachments { get; set; }    
        public DbSet<StudentDoctor> StudentDoctors { get; set; }
        public DbSet<Verification> Verifications { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<PatientAllergy> PatientAllergies { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionItem> PrescriptionItems { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Scan> Scans { get; set; }
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
                .HasMany(d => d.AIReports)
                .WithOne(r => r.Doctor)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasMany(d => d.Feedbacks)
                .WithOne(f => f.Doctor)
                .HasForeignKey(f => f.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<medicalRecord>()
    .HasOne(m => m.Appointment)
    .WithMany()
    .HasForeignKey(m => m.AppointmentId)
    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<doctor>()
                .HasMany(d => d.Verifications)
                .WithOne(v => v.Doctor)
                .HasForeignKey(v => v.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<doctor>()
                .HasMany(d => d.DoctorSchedules)
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

            builder.Entity<medicalRecord>()
    .HasOne(m => m.Appointment)
    .WithOne(a => a.MedicalRecord)
    .HasForeignKey<medicalRecord>(m => m.AppointmentId)
    .OnDelete(DeleteBehavior.NoAction);

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
        



          

            builder.Entity<StudentDoctor>()
                .HasMany(s => s.Verifications)
                .WithOne(v => v.StudentDoctor)
                .HasForeignKey(v => v.StudentDoctorId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.Entity<PatientAllergy>()
    .HasKey(pa => new { pa.PatientId, pa.AllergyId });

            builder.Entity<PatientAllergy>()
                .HasOne(pa => pa.Patient)
                .WithMany(p => p.PatientAllergies)
                .HasForeignKey(pa => pa.PatientId);

            builder.Entity<PatientAllergy>()
                .HasOne(pa => pa.Allergy)
                .WithMany(a => a.PatientAllergies)
                .HasForeignKey(pa => pa.AllergyId);

            builder.Entity<Allergy>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}

    

