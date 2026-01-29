using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }
        public string Treatment { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Notes { get; set; }
        public string Diagnosis { get; set; }

        // FK
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }


        public int? StudentDoctorId { get; set; }
        public StudentDoctor StudentDoctor { get; set; }
    }

}
