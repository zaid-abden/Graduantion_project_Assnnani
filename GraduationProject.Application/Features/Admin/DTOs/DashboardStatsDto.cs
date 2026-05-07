namespace GraduationProject.Application.Features.Admin.DTOs
{
	public record DashboardStatsDto(
	int TotalDoctors,
	int TotalPatients,
	int TotalStudents,
	int TotalReceptionists,
	int PendingRequests,
	int TotalVerified,           // إجمالي المقبولين تاريخياً
	int TotalRejected,           // إجمالي المرفوضين تاريخياً
	int TotalActionedToday,     // إجمالي (مقبول + مرفوض) اليوم
	int AppointmentsToday // الحقل الجديد
);
}
