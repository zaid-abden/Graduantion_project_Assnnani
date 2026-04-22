using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Enums
{
    public enum PaymentMethod
    {
        Cash = 1,           // كاش في العيادة
        CreditCard = 2,     // فيزا/ماستر كارد
        VodafoneCash = 3,   // تحويل محفظة
        BankTransfer = 4,   // تحويل بنكي
        Insurance = 5,      // تأمين طبي
        OnlinePayment = 6
    }
}
