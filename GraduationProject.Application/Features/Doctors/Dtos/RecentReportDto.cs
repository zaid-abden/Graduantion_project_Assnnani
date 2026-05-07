using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
    public class RecentReportDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // Scan | MedicalRecord | AI
        public string Size { get; set; }
        public string FileUrl { get; set; }
    }
}
