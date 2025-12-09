
using GraduationProject.Application.Features.Patients.Commands.CreatePatient;
using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void CreatePatientCommandMapping()
        {
            CreateMap<CreatePatientCommand, User>()
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                 .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth))
                 .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<CreatePatientCommand, Patient>()
               ;
            CreateMap<User, PatientDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName+" "+src.LastName));
                
            CreateMap<Patient, PatientDto>();
        }
    }
}


/*

Dto
   public int PatientId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string MedicalHistory { get; set; }

        public Gender Gender { get; set; }

       
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
 */


/*

{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
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

 */