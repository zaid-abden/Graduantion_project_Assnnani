namespace GraduationProject.Data.Enums
{
    public enum AppointmentStatus
    {
        Pending = 1,       // لسه الحجز متسجل لكن مش مؤكد
        Confirmed = 2,     // تأكيد الموعد (من الدكتور أو الريسيبشن)
        Completed = 3,     // المريض حضر وتمت الزيارة
        Cancelled = 4,     // المريض/الدكتور لغى الموعد
        NoShow = 5,        // المريض محضرش
        Rescheduled = 6,  // تم تغيير الموعد
        arrived = 7
    }
}
