using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Models
{
	public class StudentDoctor
	{
		public int StudentDoctorId { get; set; }
		public string University { get; set; }
		public string ImageUrl { get; set; }
		public int YearsOfStudy { get; set; }

		public string UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		public DoctorVerificationStatus VerificationStatus { get; set; } = DoctorVerificationStatus.NotSubmitted;
		public DateTime? VerifiedAt { get; set; }
		public string? RejectionReason { get; set; }

		// Relationsip
		public ICollection<medicalRecord> MedicalRecords { get; set; }

		public ICollection<AI_Report> AIReports { get; set; }

		public ICollection<Verification> Verifications { get; set; }



	}

}
