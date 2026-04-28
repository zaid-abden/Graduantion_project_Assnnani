using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class PatientAllergy
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int AllergyId { get; set; }
        public Allergy Allergy { get; set; }

        public DateTime NotedAt { get; set; }
        public string? Notes { get; set; }
    }
}
