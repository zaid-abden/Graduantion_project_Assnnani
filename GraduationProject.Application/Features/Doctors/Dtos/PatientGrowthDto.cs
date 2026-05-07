using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
    public class PatientGrowthDto
    {
        public List<string> Months { get; set; } = new();
        public List<int> Values { get; set; } = new();
    }
}
