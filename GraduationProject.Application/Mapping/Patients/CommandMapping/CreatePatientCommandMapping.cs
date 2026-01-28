
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
                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                  .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (Gender)src.Gender));

            CreateMap<CreatePatientCommand, Patient>()
               ;
            CreateMap<User, PatientDto>().ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName+" "+src.LastName))
               ;
                
            CreateMap<Patient, PatientDto>();
        }
    }
}
