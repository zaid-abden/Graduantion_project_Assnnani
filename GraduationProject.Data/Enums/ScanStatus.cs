using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public enum ScanStatus
    {
        Pending = 1,       // لسه محدش شافه
        InProgress = 2,    // الدكتور فتحه
        Reviewed = 3,      // خلص
        Rejected = 4       // اترفض
    }
}
