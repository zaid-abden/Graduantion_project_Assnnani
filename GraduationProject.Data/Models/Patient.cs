using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? MedicalHistory { get; set; }

       public Gender Gender=Gender.Male;
        
        // FK
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        // Relations
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; } 
        public ICollection<Feedback> Feedbacks { get; set; } 
        public ICollection<AI_Report> AIReports { get; set; } 
    }


}
