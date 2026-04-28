using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Prescriptions.Dtos
{
    public class PrescriptionDto
    {
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string DoctorName { get; set; }
       public DateTime Date { get; set; }
    }
}
