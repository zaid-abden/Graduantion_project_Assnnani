using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Dtos
{
    public class PatientQueueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DoctorName { get; set; }
        public TimeOnly? ArrivalTime { get; set; }
        public int? QueueNumber { get; set; }
        public string Status { get; set; }
    }
}
