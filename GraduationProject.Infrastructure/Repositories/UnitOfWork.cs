using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Repositories
{

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;

        public IAdminRepository Admins { get; }
        public IAI_ReportRepository AI_Reports { get; }
        public IAppointmentRepository Appointments { get; }
        public IDoctorRepository Doctors { get; }
        public IDoctorScheduleRepository DoctorSchedules { get; }
        public IFeedbackRepository Feedbacks { get; }
        public IMedicalRecordRepository MedicalRecords { get; }
        public IPatientRepository Patients { get; }
        public IReceptionstRepository Receptionists { get; }
        public IStudentDoctorRepository StudentDoctors { get; }
        public IVerificationRepository Verifications { get; }
        public IEmailVerificationRepository emailVerification { get; }

        public IEmailVerificationRepository EmailVerificationRepository { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IAdminRepository adminRepository,
            IAI_ReportRepository aiReportRepository,
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IDoctorScheduleRepository doctorScheduleRepository,
            IFeedbackRepository feedbackRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IPatientRepository patientRepository,
            IReceptionstRepository receptionistRepository,
            IStudentDoctorRepository studentDoctorRepository,
            IVerificationRepository verificationRepository,
            IEmailVerificationRepository emailVerification,
            IEmailVerificationRepository emailVerificationRepository
        )
        {
            _context = context;

            Admins = adminRepository;
            AI_Reports = aiReportRepository;
            Appointments = appointmentRepository;
            Doctors = doctorRepository;
            DoctorSchedules = doctorScheduleRepository;
            Feedbacks = feedbackRepository;
            MedicalRecords = medicalRecordRepository;
            Patients = patientRepository;
            Receptionists = receptionistRepository;
            StudentDoctors = studentDoctorRepository;
            Verifications = verificationRepository;
            this.emailVerification = emailVerification;
            EmailVerificationRepository= emailVerificationRepository;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}