using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class AI_Report
    {
        [Key]
        public int ReportId { get; set; }

        public string PredictionResult { get; set; }
        public double? ConfidenceScore { get; set; }

        public DateTime CreatedAt { get; set; }

        // FK
        public int ScanId { get; set; }
        public Scan Scan { get; set; }

        public int DoctorId { get; set; }
        public doctor Doctor { get; set; }
    }

}
