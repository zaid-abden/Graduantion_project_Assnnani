using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public enum PaymentStatus
    {
        Pending = 0,   // لسه ما اتدفعش
        Paid = 1,      // اتدفع بالكامل
        Failed = 2,    // فشل الدفع
        Refunded = 3,  // تم استرجاع الفلوس
        Cancelled = 4  // تم إلغاء العملية
    }

}
