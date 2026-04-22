using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,   // لسه ما اتدفعش
        Paid = 2,      // اتدفع بالكامل
        Failed = 3,    // فشل الدفع
        Refunded = 4,  // تم استرجاع الفلوس
        Cancelled = 5  // تم إلغاء العملية
    }

}
