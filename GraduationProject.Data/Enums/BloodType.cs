using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public  enum BloodType
    {
        [Description("A+")]
        A_Positive = 1,

        [Description("A-")]
        A_Negative,

        [Description("B+")]
        B_Positive,

        [Description("B-")]
        B_Negative,

        [Description("AB+")]
        AB_Positive,

        [Description("AB-")]
        AB_Negative,

        [Description("O+")]
        O_Positive,

        [Description("O-")]
        O_Negative
    }
   
}
