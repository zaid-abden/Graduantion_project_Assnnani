using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Prescription
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }

        public DateTime Date { get; set; }

        public ICollection<PrescriptionItem> Items { get; set; }
    }
}
