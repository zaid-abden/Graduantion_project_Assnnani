using GraduationProject.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Models
{
    public class Scan
    {

        public int Id { get; set; }

        public string FileUrl { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public long FileSize { get; set; }

        public ScanStatus Status { get; set; } = ScanStatus.Pending;
        public ScanType ScanType { get; set; }
        public ScanPriority Priority { get; set; } = ScanPriority.Normal;

        public DateTime UploadedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }

        public string? ReviewedBy { get; set; }
        public string? Findings { get; set; }
        public string? Recommendations { get; set; }

        public bool IsDeleted { get; set; } = false;

        // FK
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }
        public doctor Doctor { get; set; } = null!;

        // AI
        public AIStatus AIStatus { get; set; } = AIStatus.Pending;
        public AI_Report? AIReport { get; set; }
    }
}
