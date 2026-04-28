using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecord.Dtos
{
    public class MedicalHistoryDto
    {
        public string Diagnosis { get; set; }
        public DateOnly DiagnosedDate { get; set; }
       public string DoctorNotes { get; set; }
        public string CreatedBy { get; set; }
    }
}
