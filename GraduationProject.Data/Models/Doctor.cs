using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string About { get; set; }
       public string? ImageUlr { get; set; }
        public int YearsOfExperience { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Details { get; set; }
        public string Specialization { get; set; }

       

        // Relations
    public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<AI_Report> AIReports { get; set; }
        public ICollection<DoctorSchedule> Schedules { get; set; } 
        public ICollection<Verification> Verifications { get; set; } 
        public Receptionist Receptionist { get; set; }
       
        
    }

}
