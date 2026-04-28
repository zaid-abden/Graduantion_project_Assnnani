using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Dtos
{
    public class ScanDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = null!;
        public string ScanType { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
    }
}
