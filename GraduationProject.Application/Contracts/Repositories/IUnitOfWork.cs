using GraduationProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository Admins { get; }
        IAI_ReportRepository AI_Reports { get; }
       IMedicalRecordAttachmentRepository MedicalRecordAttachments { get; }
        IAppointmentRepository Appointments { get; }
        IDoctorRepository Doctors { get; }
        IDoctorScheduleRepository DoctorSchedules { get; }
        IFeedbackRepository Feedbacks { get; }
        IMedicalRecordRepository MedicalRecords { get; }
        IPatientRepository Patients { get; }
        IReceptionstRepository Receptionists { get; }
        IStudentDoctorRepository StudentDoctors { get; }
        IVerificationRepository Verifications { get; }
        ISpecializationRepository Specialization { get; }
        IScheduleSlotRepository ScheduleSlots { get; }
        IPrescriptionRepository Prescriptions { get; }
        IPrescriptionItemRepository PrescriptionItems { get; }  
        IEmailVerificationRepository EmailVerificationRepository { get; }
        IScanRepository Scans { get; }
        IPatientAllergyRepository PatientAllergies { get; }
        IAllergyRepository Allergies { get; }
        Task<int> SaveAsync();
    }
}
