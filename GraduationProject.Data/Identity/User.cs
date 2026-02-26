using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Identity
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public DateTime? BirthDate { get; set; }

        public bool IsActive { get; set; }
        public bool EmailVerified { get; set; } = false;
        public string? Gender { get; set; }
        // public string? Address { get; set; }
        // Relations
        public Receptionist Receptionist { get; set; }
        public Patient Patient { get; set; }
        public doctor Doctor { get; set; }
        public StudentDoctor StudentDoctor { get; set; }
        public ICollection<EmailVerification> EmailVerifications { get; set; }
        [InverseProperty(nameof(UserRefreshToken.user))]
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }

    }
}
