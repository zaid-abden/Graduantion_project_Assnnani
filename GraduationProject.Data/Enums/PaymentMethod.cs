using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public enum PaymentMethod
    {
        Cash = 0,           // كاش في العيادة
        CreditCard = 1,     // فيزا/ماستر كارد
        VodafoneCash = 2,   // تحويل محفظة
        BankTransfer = 3,   // تحويل بنكي
        Insurance = 4,      // تأمين طبي
        OnlinePayment = 5
    }
}
