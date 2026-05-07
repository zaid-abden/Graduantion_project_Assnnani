using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Dtos
{
    public class ReceptionistOverviewDto
    {
        public int CheckIns { get; set; }
        public int NewRegistrations { get; set; }
        public int ScheduledAppointments { get; set; }
        public int Cancellations { get; set; }
    
      
        public int CompletedAppointments { get; set; }
        public int ActiveQueue { get; set; }

       
       
    }
}
