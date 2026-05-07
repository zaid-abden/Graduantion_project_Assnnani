using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Models
{
	public class StudentDoctor
	{
		public int StudentDoctorId { get; set; }

		// 🔹 من كودك
		public string University { get; set; }

		// 🔹 من كوده
		public string NationalId { get; set; }

		public string ImageUrl { get; set; }
		public int YearsOfStudy { get; set; }

		// 🔹 العلاقة مع الدكتور (من كوده)
		public int? DoctorId { get; set; }
		public doctor? Doctor { get; set; }

		// 🔹 User (خليناها Required عشان الأمان)
		public string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		// 🔹 Verification (دمج بين الاتنين)
		public DoctorVerificationStatus VerificationStatus { get; set; } = DoctorVerificationStatus.NotSubmitted;

		public DateTime? VerifiedAt { get; set; }   // استخدمنا DateTime عشان أشمل
		public string? VerifiedBy { get; set; }
		public string? RejectionReason { get; set; }

		// 🔹 Relationships
		public ICollection<medicalRecord> MedicalRecords { get; set; } = new List<medicalRecord>();

		public ICollection<AI_Report> AIReports { get; set; } = new List<AI_Report>();

		public ICollection<Verification> Verifications { get; set; } = new List<Verification>();

		public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
	}
}