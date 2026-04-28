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
    public class medicalRecord : BaseEntity
    {
        [Key]
        public int RecordId { get; set; }

        public DateOnly VisitDate { get; set; }

        public string Title { get; set; } 
        public string? Notes { get; set; } 

        public string? Diagnosis { get; set; }

      

        
        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public ICollection<MedicalRecordAttachment> Attachments { get; set; }
    }

}
