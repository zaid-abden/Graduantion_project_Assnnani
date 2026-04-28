using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecords.Commands.Dtos
{
    public class MedicalRecordForDoctorDashboardDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string DoctorName { get; set; } = null!;
        public DateOnly Date { get; set; }
        public string Type { get; set; } = null!; // Consultation, Lab Test, Scan
        public string Description { get; set; } = null!;
        public List<AttachmentDto> Attachments { get; set; } = new();
    }
    public class AttachmentDto
    {
        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;
    }
}
