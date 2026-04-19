using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Common.Results
{
	public enum ResultStatus
	{
		Success,
		ValidationError,
		NotFound,
		Conflict,
		Unauthorized,
		Forbidden,
		Failure,
		// يمكنك إضافة أنواع جديدة هنا مستقبلاً إذا احتجت
		BadRequest = ValidationError // Alias لتوحيد المفاهيم
	}
}
