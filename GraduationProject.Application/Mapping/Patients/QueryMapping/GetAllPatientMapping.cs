using GraduationProject.Application.Features.Patients.Dtos;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Mapping.Patients
{
    public partial class PatientProfile
    {
        public void GetAllPatientMapping()
        {
            CreateMap<Data.Models.Patient, Features.Patients.Dtos.PatientDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (Gender)src.Gender));
            CreateMap<User,PatientDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
.ForMember(PatientDto=> PatientDto.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}
