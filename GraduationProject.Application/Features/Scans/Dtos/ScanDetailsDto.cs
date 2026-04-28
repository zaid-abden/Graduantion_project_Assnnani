using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Dtos
{
    public class ScanDetailsDto
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = null!;
        public string ScanType { get; set; } = null!;
       

        public string ImageUrl { get; set; } = null!;

        public DateTime UploadedAt { get; set; }

        public double? AiConfidence { get; set; }

        public string Result { get; set; } = null!;

        public string? Findings { get; set; }
        public string? Recommendations { get; set; }

        public string Status { get; set; } = null!;
    }
}
