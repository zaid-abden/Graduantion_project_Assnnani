using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Dtos
{
    public class PatientForDoctorDashboardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public DateOnly? LastVisit { get; set; }
       
    }
}
