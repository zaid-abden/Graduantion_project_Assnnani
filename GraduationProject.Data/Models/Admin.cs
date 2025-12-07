using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        // Relations
        public ICollection<Verification> Verifications { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

    }

}
