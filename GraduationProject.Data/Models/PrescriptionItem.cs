using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class PrescriptionItem
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

        public string MedicationName { get; set; }
        public string Dosage { get; set; }       // 10mg
        public string Frequency { get; set; }    // twice daily
        public int DurationInDays { get; set; }
    }
}
