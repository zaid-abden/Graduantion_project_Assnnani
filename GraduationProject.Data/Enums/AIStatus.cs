using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public enum AIStatus
    {
        Pending = 0,     // لسه متعملش processing
        Processing = 1,  // شغال
        Completed = 2,   // خلص
        Failed = 3       // حصل error
    }
}
