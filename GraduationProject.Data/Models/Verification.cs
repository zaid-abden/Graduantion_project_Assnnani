using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Verification
    {
        public int VerificationId { get; set; }
        public string? LicenseNumber { get; set; }
        public string ImageUlr { get; set; }
        public string VerificationStatus { get; set; }
        public DateTime ReviewedAt { get; set; }

        // FK
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        public Admin Admin { get; set; }
    }

}
