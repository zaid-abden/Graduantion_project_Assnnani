using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Models
{
    [Table("Doctors")]
    public class doctor
    {
        public int DoctorId { get; set; }
        public string? FullName { get; set; }
        public string MedicalLicenseNumber { get; set; } = null!;
        public string? ClinicName { get; set; }
        public string? ClinicLocation { get; set; }
        // Profile
        public string? About { get; set; }
        public string? ImageUrl { get; set; }
        public string? DoctorCertificate { get; set; } = null!;
        public int YearsOfExperience { get; set; }

        // Address
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Details { get; set; }

        // Medical Info
        public DoctorDegree Degree { get; set; }

        // Verification
        public DoctorVerificationStatus VerificationStatus { get; set; }
            = DoctorVerificationStatus.NotSubmitted;

        public DateTime? VerifiedAt { get; set; }
        public string? RejectionReason { get; set; }

        // Statistics
        public int NumberOfPatientsSeen { get; set; } = 0;
        public double Rating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;

        public string? ClinicPhoneNumber { get; set; }

        public string? Education { get; set; }
        public List<string>? Languages { get; set; }
        public int? SpecializationId { get; set; }
        public Specialization Specialization { get; set; }


        // Relations
        [ForeignKey(nameof(UserId))]

        public User User { get; set; }

        public string UserId { get; set; }
        public ICollection<medicalRecord> MedicalRecords { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<AI_Report> AIReports { get; set; }
       
        public ICollection<Verification> Verifications { get; set; }
        public ICollection<doctorSchedule> DoctorSchedules { get; set; } = new List<doctorSchedule>();
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public Receptionist Receptionist { get; set; }
        public ICollection<Patient> Patients { get; set; } = new HashSet<Patient>();
        public decimal? price { get; set; }
        //public decimal? Price { get; set; }
        public int YearsOfService { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }

}

