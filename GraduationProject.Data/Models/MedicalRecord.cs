using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class MedicalRecord : BaseEntity
    {
        [Key]
        public int RecordId { get; set; }

        [Required, MaxLength(200)]
        public string Treatment { get; set; } = string.Empty;

        [Required]
        public DateTime VisitDate { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(300)]
        public string? Diagnosis { get; set; }

      
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

      
        public int DoctorId { get; set; }
        public doctor Doctor { get; set; } = null!;

       
        public int? StudentDoctorId { get; set; }
        public StudentDoctor? StudentDoctor { get; set; }
    }

}
