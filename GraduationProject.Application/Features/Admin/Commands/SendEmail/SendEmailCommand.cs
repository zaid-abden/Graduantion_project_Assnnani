using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.SendEmail
{
	public record SendEmailCommand(
	string Subject,
	string Body,
	string? UserId = null,         // إرسال لمستخدم واحد
	string? RoleName = null,       // إرسال لـ Role كامل
	List<string>? UserIds = null   // إرسال لمجموعة مختارة
) : IRequest<Result<bool>>;
}
