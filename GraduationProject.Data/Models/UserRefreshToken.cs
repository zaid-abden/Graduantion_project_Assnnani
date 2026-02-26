using GraduationProject.Data.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Models
{
    public class UserRefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? JTI { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime Created { get; set; }
        public DateTime ExpiryDate { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(User.UserRefreshTokens))]
        public virtual User? user { get; set; }
    }
}
