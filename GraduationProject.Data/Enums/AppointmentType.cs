using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GraduationProject.Data.Enums
{
   

    public enum AppointmentType
    {
        [Display(Name = "Consultation")]
        Consultation = 1,

        [Display(Name = "Follow Up")]
        FollowUp = 2,

        [Display(Name = "General Checkup")]
        Checkup = 3,

        [Display(Name = "Emergency")]
        Emergency = 4
    }
}
