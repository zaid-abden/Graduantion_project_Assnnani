using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Financial.DTOs;
using GraduationProject.Data.Enums; // لكي يتعرف على الـ Enums
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{
	public class FinancialRepository : IFinancialRepository
	{
		private readonly ApplicationDbContext _context;
		public FinancialRepository(ApplicationDbContext context) => _context = context;

		public async Task<FinancialReportResponse> GetDoctorFinancialReportAsync(int doctorId, DateTime start, DateTime end)
		{
			// ملاحظة: بما أن الموعد مرتبط بالجدول والجدول مرتبط بالدكتور
			// سنصل للدكتور عن طريق a.DoctorSchedule.DoctorId

			var appointments = await _context.Appointments
				.Include(a => a.Patient)
					.ThenInclude(p => p.User) // نحتاج الـ User لنجلب اسم المريض
				.Include(a => a.DoctorSchedule)
				.Where(a => a.DoctorSchedule.DoctorId == doctorId
						   && a.AppointmentStatus == AppointmentStatus.Completed // تأكد من وجود Completed في الـ Enum
						   && a.AppointmentTime >= start
						   && a.AppointmentTime <= end)
				.ToListAsync();

			var response = new FinancialReportResponse
			{
				// افترضنا سعر ثابت (مثلاً 100) لعدم وجود حقل Price في كلاس Appointment
				TotalEarnings = appointments.Count * 100,
				TotalCompletedAppointments = appointments.Count,

				TransactionList = appointments.Select(a => new TransactionDto
				{
					AppointmentId = a.AppointmentId,
					// الاسم موجود في كلاس الـ User المرتبط بالـ Patient
					PatientName = a.Patient?.User?.FirstName + " " + a.Patient?.User?.LastName ?? "Unknown Patient",
					Date = a.AppointmentTime,
					Amount = 100,
					Status = a.AppointmentStatus.ToString()
				}).ToList(),

				ChartData = appointments
					.GroupBy(a => a.AppointmentTime.ToString("MMMM"))
					.Select(g => new MonthlyChartData
					{
						MonthName = g.Key,
						Earnings = g.Count() * 100
					}).ToList()
			};

			return response;
		}
	}
}