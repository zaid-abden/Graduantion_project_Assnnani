using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Data.Identity
{
	public class User : IdentityUser
	{

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName => $"{FirstName} {LastName}";

		public DateTime? BirthDate { get; set; }
		public string? ImageUrl { get; set; }
		public bool IsActive { get; set; }
		public bool EmailVerified { get; set; } = false;
		public string? Gender { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; } // تاريخ آخر عملية تمت على الحساب
		public bool IsDeleted { get; set; } = false; // أضف هذا السطر
													 // public string? Address { get; set; }
													 // Relations
		public Receptionist Receptionist { get; set; }
		public Patient Patient { get; set; }
		public doctor Doctor { get; set; }
		public StudentDoctor StudentDoctor { get; set; }
		public ICollection<EmailVerification> EmailVerifications { get; set; }
	}
}
