using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using GraduationProject.Application.Features.Admin.Queries.GetAllUsers;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{
	public class AdminRepository
		 : GenericRepository<Admin>, IAdminRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AdminRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext)
		{
			_context = dbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<List<doctor>> GetPendingDoctorsOnlyAsync()
		{
			return await _context.Doctors
				.Include(d => d.User)
				.Where(d => d.VerificationStatus == DoctorVerificationStatus.Pending && !d.User.IsDeleted)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<doctor?> GetPendingDoctorByIdAsync(int doctorId)
		{
			return await _context.Doctors
				.Include(d => d.User)
				.FirstOrDefaultAsync(d => d.DoctorId == doctorId
									 && d.VerificationStatus == DoctorVerificationStatus.Pending
									 && !d.User.IsDeleted);
		}

		public async Task<(List<doctor> Doctors, int TotalCount)> FilterPendingDoctorsAsync(string? searchTerm, int pageNumber, int pageSize)
		{
			// 1. استرجاع الدكاترة الـ Pending فقط (بدون طلاب)
			var query = _context.Doctors
				.Include(d => d.User)
				.Where(d => d.VerificationStatus == DoctorVerificationStatus.Pending && !d.User.IsDeleted)
				.AsNoTracking();

			// 2. هندلة الـ SearchTerm (null, "", أو مسافات)
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				var term = searchTerm.Trim().ToLower();
				query = query.Where(d =>
					(d.User.FirstName + " " + d.User.LastName).ToLower().Contains(term) ||
					d.User.Email.ToLower().Contains(term) ||
					(d.User.PhoneNumber != null && d.User.PhoneNumber.Contains(term)) ||
					d.MedicalLicenseNumber.Contains(term)
				);
			}

			var totalCount = await query.CountAsync();

			var doctors = await query
				.OrderByDescending(d => d.User.CreatedAt)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return (doctors, totalCount);
		}

		public async Task<string?> ApproveUserAsync(string id)
		{
			// 1. التحقق من أن الـ id المرسل هو رقم صحيح (int)
			if (!int.TryParse(id, out int doctorId))
			{
				return null; // أو يمكنك رمي Exception إذا كنت تفضل ذلك
			}

			// 2. البحث باستخدام الـ doctorId بعد تحويله
			var doctor = await _context.Doctors
				.Include(d => d.User)
				.FirstOrDefaultAsync(d => d.DoctorId == doctorId);

			if (doctor == null || doctor.User == null) return null;

			// 3. تحديث البيانات
			doctor.VerificationStatus = DoctorVerificationStatus.Approved;
			doctor.VerifiedAt = DateTime.UtcNow;

			doctor.User.IsActive = true;
			doctor.User.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
			return doctor.User.Email;
		}

		public async Task<string?> RejectUserAsync(string id, string reason)
		{
			// 1. التحقق من صحة الـ id
			if (!int.TryParse(id, out int doctorId))
			{
				return null;
			}

			// 2. البحث في جدول الدكاترة
			var doctor = await _context.Doctors
				.Include(d => d.User)
				.FirstOrDefaultAsync(d => d.DoctorId == doctorId);

			if (doctor == null || doctor.User == null) return null;

			// 3. تحديث الحالة وسبب الرفض
			doctor.VerificationStatus = DoctorVerificationStatus.Rejected;
			doctor.RejectionReason = reason;
			doctor.VerifiedAt = DateTime.UtcNow;

			doctor.User.IsActive = false;
			doctor.User.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
			return doctor.User.Email;
		}

		public async Task<List<doctor>> GetRejectedDoctorsOnlyAsync()
		{
			// بنجيب الدكاترة المرفوضين بس
			return await _context.Doctors
				.Include(d => d.User)
				.Where(d => d.VerificationStatus == DoctorVerificationStatus.Rejected && !d.User.IsDeleted)
				.OrderByDescending(d => d.VerifiedAt) // تاريخ الرفض
				.AsNoTracking()
				.ToListAsync();
		}
		public async Task<DashboardStatsDto> GetDashboardStatsAsync()
		{
			// 1. تحويل التاريخ الحالي إلى DateOnly
			var todayDateTime = DateTime.UtcNow;
			var todayDateOnly = DateOnly.FromDateTime(todayDateTime);
			var todayDate = todayDateTime.Date; // بنحتاجه للـ VerifiedAt لأنه DateTime

			// إحصائيات الأطباء
			var totalVerifiedDoctors = await _context.Doctors
				.CountAsync(d => d.VerificationStatus == DoctorVerificationStatus.Approved);

			var totalRejectedDoctors = await _context.Doctors
				.CountAsync(d => d.VerificationStatus == DoctorVerificationStatus.Rejected);

			var verifiedToday = await _context.Doctors
				.CountAsync(d => d.VerifiedAt.HasValue && d.VerifiedAt.Value.Date == todayDate);

			// 2. المقارنة باستخدام DateOnly
			var appointmentsToday = await _context.Appointments
				.Where(a => !a.IsDeleted)
				.CountAsync(a => _context.ScheduleSlots
					.Any(s => s.Id == a.ScheduleSlotId && s.Date == todayDateOnly)); // هنا التعديل

			return new DashboardStatsDto(
				TotalDoctors: await _context.Doctors.CountAsync(),
				TotalPatients: await _context.Patients.CountAsync(),
				TotalStudents: await _context.StudentDoctors.CountAsync(),
				TotalReceptionists: await _context.Receptionists.CountAsync(),
				PendingRequests: await _context.Doctors.CountAsync(d => d.VerificationStatus == DoctorVerificationStatus.Pending),
				TotalVerified: totalVerifiedDoctors,
				TotalRejected: totalRejectedDoctors,
				TotalActionedToday: verifiedToday,
				AppointmentsToday: appointmentsToday
			);
		}

		public async Task<PagedUsersDto> GetAllUsersAsync(GetAllUsersQuery filter)
		{
			var request = _httpContextAccessor.HttpContext.Request;
			var baseUrl = $"{request.Scheme}://{request.Host}";

			// تنظيف بسيط للمسافات فقط لأن الـ JSON بيحمي الداتا من رموز الـ URL
			string? searchTerm = string.IsNullOrWhiteSpace(filter.SearchTerm) ? null : filter.SearchTerm.Trim().ToLower();
			string? role = string.IsNullOrWhiteSpace(filter.Role) ? null : filter.Role.Trim().ToLower();
			string? gender = string.IsNullOrWhiteSpace(filter.Gender) ? null : filter.Gender.Trim().ToLower();

			var query = _context.Users.AsNoTracking().AsQueryable();

			// استبعاد الأدمن
			query = query.Where(u => !_context.UserRoles
				.Any(ur => ur.UserId == u.Id &&
					 _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Admin")));

			// فلتر البحث النصي
			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(u =>
					(u.FirstName + " " + u.LastName).ToLower().Contains(searchTerm) ||
					u.Email.ToLower().Contains(searchTerm) ||
					(u.PhoneNumber != null && u.PhoneNumber.Contains(searchTerm)));
			}

			// فلتر الـ Role
			if (!string.IsNullOrEmpty(role))
			{
				query = query.Where(u => _context.UserRoles
					.Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name.ToLower() == role)));
			}

			// فلتر الـ Gender
			if (!string.IsNullOrEmpty(gender))
			{
				query = query.Where(u => u.Gender != null && u.Gender.ToLower() == gender);
			}

			var totalCount = await query.CountAsync();

			var users = await query
				.OrderByDescending(u => u.CreatedAt)
				.Skip((filter.PageNumber - 1) * filter.PageSize)
				.Take(filter.PageSize)
				.Select(u => new UserListItemDto(
					u.Id,
					u.FirstName + " " + u.LastName,
					u.Email,
					u.PhoneNumber,
					_context.UserRoles.Where(ur => ur.UserId == u.Id)
						.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
						.FirstOrDefault() ?? "No Role",
					u.IsActive,
					u.Gender ?? "Not Specified",
					u.CreatedAt,
					!string.IsNullOrEmpty(u.ImageUrl)
						? $"{baseUrl}/uploads/images/{u.ImageUrl}"
						: $"{baseUrl}/uploads/images/default-user-image.png"
				)).ToListAsync();

			return new PagedUsersDto(users, totalCount);
		}

		public async Task<User?> GetUserByIdAsync(string id)
		{
			return await _context.Users
				.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
		}

		public async Task<List<doctor>> GetDoctorsByStatusAsync(DoctorVerificationStatus? status)
		{
			var query = _context.Doctors
				.Include(d => d.User)
				.Where(d => !d.User.IsDeleted)
				.AsNoTracking();

			if (status.HasValue)
			{
				query = query.Where(d => d.VerificationStatus == status.Value);
			}

			return await query.ToListAsync(); // هيرجع List من doctor
		}

		// جلب الأطباء مع التخصص ورقم الرخصة
		public async Task<List<doctor>> GetDoctorsOnlyAsync()
		{
			return await _context.Doctors
				.Include(d => d.User)
				.Where(d => !d.User.IsDeleted)
				.AsNoTracking()
				.ToListAsync();
		}

		// جلب المرضى مع ملخص التاريخ الطبي
		public async Task<List<PatientListDto>> GetPatientsOnlyAsync()
		{
			return await _context.Patients
				.AsNoTracking() // <--- إضافة مهمة جداً للـ Queries
				.Select(p => new PatientListDto(
					p.UserId,
					p.User.FirstName + " " + p.User.LastName,
					p.User.Email,
					//p.MedicalHistory,
					p.User.IsActive
				)).ToListAsync();
		}

		// جلب الطلاب مع الجامعة وسنة الدراسة
		public async Task<List<StudentListDto>> GetStudentsOnlyAsync()
		{
			return await _context.StudentDoctors
				.AsNoTracking() // لتحسين الأداء وسرعة القراءة
				.Select(s => new StudentListDto(
					s.UserId,
					s.User.FirstName + " " + s.User.LastName,
					s.User.Email,
					s.University,
					s.YearsOfStudy,
					s.User.IsActive
				)).ToListAsync();
		}

		// جلب موظفي الاستقبال مع اسم الدكتور التابعين له
		public async Task<List<ReceptionistListDto>> GetReceptionistsOnlyAsync()
		{
			return await _context.Receptionists
				.AsNoTracking()
				.Select(r => new ReceptionistListDto(
					r.UserId,
					r.User.FirstName + " " + r.User.LastName,
					r.User.Email,
					// حماية في حالة لو الـ Doctor أو الـ User بتاعه مش موجودين
					(r.Doctor != null && r.Doctor.User != null)
						? r.Doctor.User.FirstName + " " + r.Doctor.User.LastName
						: "Not Assigned",
					r.Shift.ToString(),
					r.User.IsActive
				)).ToListAsync();
		}

		public async Task<List<string>> GetEmailsByTargetAsync(string? userId, string? roleName, List<string>? userIds)
		{
			var emailList = new List<string>();

			// حالة 1: مستخدم واحد
			if (!string.IsNullOrEmpty(userId))
			{
				var email = await _context.Users.Where(u => u.Id == userId).Select(u => u.Email).FirstOrDefaultAsync();
				if (email != null) emailList.Add(email);
			}
			// حالة 2: Role معين (مثل الأطباء فقط)
			else if (!string.IsNullOrEmpty(roleName))
			{
				emailList = await _context.Users
					.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id &&
								_context.Roles.Any(r => r.Id == ur.RoleId && r.Name == roleName)))
					.Select(u => u.Email)
					.ToListAsync();
			}
			// حالة 3: قائمة مستخدمين محددة
			else if (userIds != null && userIds.Any())
			{
				emailList = await _context.Users
					.Where(u => userIds.Contains(u.Id))
					.Select(u => u.Email)
					.ToListAsync();
			}

			return emailList.Distinct().ToList(); // ضمان عدم التكرار
		}

		public async Task<bool> ToggleUserStatusAsync(string userId)
		{
			var user = await _context.Users.FindAsync(userId);

			if (user == null) return false;

			// عكس الحالة الحالية
			user.IsActive = !user.IsActive;
			user.UpdatedAt = DateTime.UtcNow;

			// تحديث قاعدة البيانات
			_context.Users.Update(user);
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
