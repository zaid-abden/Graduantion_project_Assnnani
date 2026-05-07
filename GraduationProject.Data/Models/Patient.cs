using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
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

        //public string? MedicalHistory { get; set; }

        public Gender Gender { get; set; } = Gender.Male;

        public PatientStatus Status { get; set; } = PatientStatus.Pending;

     

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public BloodType? BloodType { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public ICollection<PatientAllergy> PatientAllergies { get; set; }


        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int? AssignedDoctorId { get; set; }

        [ForeignKey(nameof(AssignedDoctorId))]
        public doctor? AssignedDoctor { get; set; }



        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<medicalRecord> MedicalRecords { get; set; } = new List<medicalRecord>();

        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public ICollection<AI_Report> AIReports { get; set; } = new List<AI_Report>();
    }


}


/*
 *  public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
  
        public User User { get; set; }
 * 
 */