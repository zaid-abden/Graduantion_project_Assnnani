using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Dtos
{
    public class ReceptionistDashboardDto
    {
        public int Appointments { get; set; }
        public int InQueue { get; set; }
        public int TotalPatients { get; set; }
    }
}
