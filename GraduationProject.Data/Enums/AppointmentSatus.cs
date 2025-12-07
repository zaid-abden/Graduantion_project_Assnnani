using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public enum AppointmentStatus
    {
        Pending = 0,       // لسه الحجز متسجل لكن مش مؤكد
        Confirmed = 1,     // تأكيد الموعد (من الدكتور أو الريسيبشن)
        Completed = 2,     // المريض حضر وتمت الزيارة
        Cancelled = 3,     // المريض/الدكتور لغى الموعد
        NoShow = 4,        // المريض محضرش
        Rescheduled = 5    // تم تغيير الموعد
    }
}
