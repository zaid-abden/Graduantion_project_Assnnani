using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Prescriptions.Dtos
{
    public class CreatePrescriptionDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public List<CreatePrescriptionItemDto> Items { get; set; } = new();
    }
}
