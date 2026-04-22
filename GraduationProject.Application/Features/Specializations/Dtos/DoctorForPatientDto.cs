using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Dtos
{

    public class DoctorForPatientDto
    {
        
        public string FullName { get; set; } = null!;
        public string? ClinicName { get; set; }
        public string SpecializationName { get; set; } = null!;
        public double Rating { get; set; }
        public int RatingCount { get; set; }
        public int YearsOfExperience { get; set; }

        public List<DoctorScheduleDtoForPatient>? Schedules { get; set; } // لو عايز تعرض أوقات العمل
    }

}
