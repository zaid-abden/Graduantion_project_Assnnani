using GraduationProject.Data.Enums;
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



        // Basic Information
        public string About { get; set; }
        public string? ImageUrl { get; set; }
        public int YearsOfExperience { get; set; }



        // Address
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Details { get; set; }




        // Medical Information
        //public string Specialization { get; set; }
        public DoctorDegree Degree { get; set; }



        // Statistics
        public int NumberOfPatientsSeen { get; set; } = 0;

        // Rating
        public double Rating { get; set; } = 0;       
        public int RatingCount { get; set; } = 0;


        // Optional Clinic Contact
        public string? ClinicPhoneNumber { get; set; }




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


/*
doctor.Rating = 
    ((doctor.Rating * doctor.RatingCount) + newRateValue) 
    / (doctor.RatingCount + 1);
doctor.RatingCount++;
 */