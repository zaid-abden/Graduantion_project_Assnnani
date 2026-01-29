using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctor
{
    public class SubmitDoctorVerificationCommand
    {
        public string MedicalLicenseNumber { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public int SpecializationId { get; set; }
        public int YearsOfExperience { get; set; }
        public string ClinicName { get; set; } = null!;
        public string ClinicAddress { get; set; } = null!;
        public string ClinicPhone { get; set; } = null!;

        public IFormFile Certificate { get; set; } = null!;
    }
}
