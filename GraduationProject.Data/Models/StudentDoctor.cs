using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class StudentDoctor
    {
        public int StudentDoctorId { get; set; }
        public string University { get; set; }
       public string ImageUrl { get; set; }
        public int YearsOfStudy { get; set; }
        // FK
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        // Relationsip
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        
        public ICollection<AI_Report> AIReports { get; set; }
      
        public ICollection<Verification> Verifications { get; set; }
   


    }

}
