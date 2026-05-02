using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using GraduationProject.Application.Features.Admin.Queries.GetAllUsers;
using GraduationProject.Data.Enums;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{
	public class AdminRepository
		 : GenericRepository<Admin>, IAdminRepository
	{
		private readonly ApplicationDbContext _context;

		public AdminRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_context = dbContext;
		}

		public async Task<List<PendingUserDto>> GetPendingUsersAsync()
		{
			return await _context.Users
				.Where(u => !u.IsActive)
				.Select(u => new PendingUserDto(
					u.Id,
					u.FirstName + " " + u.LastName,
					u.Email,
					_context.UserRoles.Where(ur => ur.UserId == u.Id)
						.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
						.FirstOrDefault() ?? "Unknown",
					u.StudentDoctor != null ? u.StudentDoctor.University : null,
					u.StudentDoctor != null ? u.StudentDoctor.YearsOfStudy : null,
					u.Doctor != null ? u.Doctor.MedicalLicenseNumber : null,
					u.Doctor != null ? u.Doctor.Specialization.Name : null,
					u.ImageUrl,
					null // You can map a CreatedAt property if it exists in AspNetUsers
				)).ToListAsync();
		}

		public async Task<string?> ApproveUserAsync(string userId)
		{
			var user = await _context.Users
				.Include(u => u.Doctor)
				.Include(u => u.StudentDoctor)
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null || user.IsActive) return null;

			// 1. تحديث الحالة في جدول الهوية الأساسي
			user.IsActive = true;
			user.UpdatedAt = DateTime.UtcNow;

			// 2. تحديث الحالة في جدول الطبيب أو الطالب
			if (user.Doctor != null)
			{
				user.Doctor.VerificationStatus = DoctorVerificationStatus.Approved;
				user.Doctor.VerifiedAt = DateTime.UtcNow;
			}
			else if (user.StudentDoctor != null)
			{
				user.StudentDoctor.VerificationStatus = DoctorVerificationStatus.Approved;
				user.StudentDoctor.VerifiedAt = DateTime.UtcNow;
			}

			await _context.SaveChangesAsync();
			return user.Email;
		}

		public async Task<string?> RejectUserAsync(string userId, string reason)
		{
			var user = await _context.Users
				.Include(u => u.Doctor)
				.Include(u => u.StudentDoctor)
				.FirstOrDefaultAsync(u => u.Id == userId);

			if (user == null) return null;

			// تحديث الحالة في جدول الطبيب أو الطالب
			if (user.Doctor != null)
			{
				user.Doctor.VerificationStatus = DoctorVerificationStatus.Rejected;
				user.Doctor.RejectionReason = reason;
				user.Doctor.VerifiedAt = DateTime.UtcNow; // تاريخ اتخاذ القرار
			}
			else if (user.StudentDoctor != null)
			{
				user.StudentDoctor.VerificationStatus = DoctorVerificationStatus.Rejected;
				user.StudentDoctor.RejectionReason = reason;
				user.StudentDoctor.VerifiedAt = DateTime.UtcNow;
			}

			// لا نحذف المستخدم، فقط نغلق الحساب ونحدث التوقيت
			user.IsActive = false;
			user.UpdatedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
			return user.Email; // نرجع الإيميل عشان الـ Handler يبعت الرسالة
		}

		public async Task<List<RejectedUserDto>> GetRejectedUsersAsync()
		{
			// جلب المرفوضين من جدول الأطباء
			var rejectedDoctors = await _context.Doctors
				.Where(d => d.VerificationStatus == DoctorVerificationStatus.Rejected)
				.Select(d => new RejectedUserDto(
					d.UserId,
					d.User.FirstName + " " + d.User.LastName,
					d.User.Email,
					"Doctor",
					d.RejectionReason ?? "No reason provided",
					d.VerifiedAt, // نستخدم VerifiedAt كتاريخ للرفض أيضاً
					null,
					d.MedicalLicenseNumber
				)).ToListAsync();

			// جلب المرفوضين من جدول الطلاب
			var rejectedStudents = await _context.StudentDoctors
				.Where(s => s.VerificationStatus == DoctorVerificationStatus.Rejected)
				.Select(s => new RejectedUserDto(
					s.UserId,
					s.User.FirstName + " " + s.User.LastName,
					s.User.Email,
					"StudentDoctor",
					s.RejectionReason ?? "No reason provided",
					s.VerifiedAt,
					s.University,
					null
				)).ToListAsync();

			// دمج القائمتين وترتيبهم من الأحدث للأقدم
			return rejectedDoctors.Concat(rejectedStudents)
				.OrderByDescending(x => x.RejectedAt)
				.ToList();
		}

		public async Task<DashboardStatsDto> GetDashboardStatsAsync()
		{
			var today = DateTime.UtcNow.Date;

			// إحصائيات من جدول الأطباء مباشرة باستخدام الـ Enum بتاعك
			var totalVerifiedDoctors = await _context.Doctors
				.CountAsync(d => d.VerificationStatus == DoctorVerificationStatus.Approved);

			var totalRejectedDoctors = await _context.Doctors
				.CountAsync(d => d.VerificationStatus == DoctorVerificationStatus.Rejected);

			// العمليات اللي تمت النهاردة
			var verifiedToday = await _context.Doctors
				.CountAsync(d => d.VerifiedAt.HasValue && d.VerifiedAt.Value.Date == today);

			// ملاحظة: لو مفيش جدول طلبات مرفوضة مستقل، الـ RejectionReason 
			// هو اللي هيعرفنا مين اترفض النهاردة (لو ضفت حقل RejectedAt)

			return new DashboardStatsDto(
				TotalDoctors: await _context.Doctors.CountAsync(),
				TotalPatients: await _context.Patients.CountAsync(),
				TotalStudents: await _context.StudentDoctors.CountAsync(),
				TotalReceptionists: await _context.Receptionists.CountAsync(),

				// المستخدمين اللي حالتهم Pending في جدول الطبيب
				PendingRequests: await _context.Doctors.CountAsync(d => d.VerificationStatus == DoctorVerificationStatus.Pending),

				TotalVerified: totalVerifiedDoctors,
				TotalRejected: totalRejectedDoctors,
				TotalActionedToday: verifiedToday // + مرفوضين اليوم لو ضفت RejectedAt
			);
		}

		public async Task<PagedUsersDto> GetAllUsersAsync(GetAllUsersQuery filter)
		{
			var query = _context.Users
		.Include(u => u.Doctor)
		.Include(u => u.StudentDoctor)
		.Include(u => u.Patient)
		.AsQueryable();

			// 1. Search Filter (Name, Email, Phone)
			if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
			{
				var term = filter.SearchTerm.ToLower();
				query = query.Where(u => u.FirstName.ToLower().Contains(term) ||
										 u.LastName.ToLower().Contains(term) ||
										 u.Email.ToLower().Contains(term) ||
										 u.PhoneNumber.Contains(term));
			}

			// 2. Role Filter
			if (!string.IsNullOrWhiteSpace(filter.Role))
			{
				query = query.Where(u => _context.UserRoles
					.Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == filter.Role)));
			}

			// 3. Gender Filter
			if (!string.IsNullOrWhiteSpace(filter.Gender))
			{
				query = query.Where(u => u.Gender == filter.Gender);
			}

			// 4. Status Filter (بناءً على التعديلات الأخيرة في كلاس الـ User)
			if (!string.IsNullOrWhiteSpace(filter.Status))
			{
				// إذا كان المستخدم يبحث عن حالة "Approved" أو "Pending" للأطباء/الطلاب
				if (Enum.TryParse<DoctorVerificationStatus>(filter.Status, true, out var docStatus))
				{
					query = query.Where(u =>
						(u.Doctor != null && u.Doctor.VerificationStatus == docStatus) ||
						(u.StudentDoctor != null && u.StudentDoctor.VerificationStatus == docStatus));
				}
				// أو إذا كان يبحث عن حالة المريض
				else if (Enum.TryParse<PatientStatus>(filter.Status, true, out var patStatus))
				{
					query = query.Where(u => u.Patient != null && u.Patient.Status == patStatus);
				}
			}

			// حساب العدد الإجمالي قبل الـ Pagination
			var totalCount = await query.CountAsync();

			// تطبيق الـ Pagination والـ Projection
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
						.FirstOrDefault() ?? "Unknown",
					u.IsActive,
					u.Gender,
					u.CreatedAt,
					u.ImageUrl
				)).ToListAsync();

			return new PagedUsersDto(users, totalCount);
		}

		// جلب الأطباء مع التخصص ورقم الرخصة
		public async Task<List<DoctorListDto>> GetDoctorsOnlyAsync()
		{
			return await _context.Doctors
				.Select(d => new DoctorListDto(
					d.UserId,
					d.User.FirstName + " " + d.User.LastName,
					d.User.Email,
					d.Specialization.Name,
					d.MedicalLicenseNumber,
					d.User.IsActive
				)).ToListAsync();
		}

		// جلب المرضى مع ملخص التاريخ الطبي
		public async Task<List<PatientListDto>> GetPatientsOnlyAsync()
		{
			return await _context.Patients
				.Select(p => new PatientListDto(
					p.UserId,
					p.User.FirstName + " " + p.User.LastName,
					p.User.Email,
					p.MedicalHistory,
					p.User.IsActive
				)).ToListAsync();
		}

		// جلب الطلاب مع الجامعة وسنة الدراسة
		public async Task<List<StudentListDto>> GetStudentsOnlyAsync()
		{
			return await _context.StudentDoctors
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
				.Select(r => new ReceptionistListDto(
					r.UserId,
					r.User.FirstName + " " + r.User.LastName,
					r.User.Email,
					r.Doctor.User.FirstName + " " + r.Doctor.User.LastName, // اسم الدكتور من جدول اليوزر المرتبط بجدول الدكتور
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
