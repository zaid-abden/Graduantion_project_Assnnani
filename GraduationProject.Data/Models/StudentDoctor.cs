using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Models
{
	public class StudentDoctor
	{
		public int StudentDoctorId { get; set; }

		
		public string University { get; set; }

		
		public string NationalId { get; set; }
		public string ? CertificationDocument { get; set; }
		public string ? Note { get; set; }
        public string? ImageUrl { get; set; }
		public int YearsOfStudy { get; set; }


	public string? ClinicName { get; set; }
		public string? ClinicLocation { get; set; }
		public string? Notes { get; set; }
        public int? DoctorId { get; set; }
		public doctor? Doctor { get; set; }

		
		public string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		
		public DoctorVerificationStatus VerificationStatus { get; set; } = DoctorVerificationStatus.NotSubmitted;
        public StudentDoctorStatus Status { get; set; }
            = StudentDoctorStatus.PendingReview;
        public DateTime? VerifiedAt { get; set; } 
		public string? VerifiedBy { get; set; }
		public string? RejectionReason { get; set; }

		  public string ? SupervisingNumber { get; set; }
        public ICollection<medicalRecord> MedicalRecords { get; set; } = new List<medicalRecord>();

		public ICollection<AI_Report> AIReports { get; set; } = new List<AI_Report>();

		public ICollection<Verification> Verifications { get; set; } = new List<Verification>();

		public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
	}
}