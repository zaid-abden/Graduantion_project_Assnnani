using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Dtos
{
    public class InsightItemDto
    {
        public decimal Value { get; set; }
        public  double Change { get; set; }
    }

    public class InsightsDto
    {
        public InsightItemDto TotalPatients { get; set; }
        public InsightItemDto Appointment { get; set; }
        public InsightItemDto ScanProcessed { get; set; }
        public InsightItemDto Revenue { get; set; }
    }
}
