using GraduationProject.Application.Features.Appointments.Dtos;
using GraduationProject.Application.Features.MedicalRecord.Dtos;
using GraduationProject.Application.Features.Prescriptions.Dtos;
using GraduationProject.Application.Features.Receptionist.Queries.PatientInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Dtos
{
    public class PatientDetailsDto
    {
        public PersonalInfoDto PersonalInfo { get; set; }
        public List<string> Allergies { get; set; }
        public List<MedicalHistoryDto> MedicalHistories { get; set; }
        public List<PrescriptionDto> Prescriptions { get; set; }
        public List<AppointmentDtoForDoctorDashboard> Appointments { get; set; }
    }
}
