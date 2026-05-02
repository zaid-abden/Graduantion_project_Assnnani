using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.RejectUser
{
	public class RejectUserHandler : IRequestHandler<RejectUserCommand, Result<bool>>
	{
		private readonly IAdminRepository _adminRepository;
		private readonly IEmailService _emailService;

		public RejectUserHandler(IAdminRepository adminRepository, IEmailService emailService)
		{
			_adminRepository = adminRepository;
			_emailService = emailService;
		}

		public async Task<Result<bool>> Handle(RejectUserCommand request, CancellationToken cancellationToken)
		{
			// 1. تحديث قاعدة البيانات (تغيير الحالة لـ Rejected وحفظ السبب)
			var userEmail = await _adminRepository.RejectUserAsync(request.Id, request.Reason);

			if (string.IsNullOrEmpty(userEmail))
			{
				return Result<bool>.Failure(ResultStatus.NotFound, "User not found or already processed.");
			}

			// 2. إرسال الإيميل تلقائياً
			string subject = "Update Regarding Your Asnanii Account";
			string body = $@"
            <h3>Dear Doctor,</h3>
            <p>We have reviewed your application for <b>Asnanii Clinic Management System</b>.</p>
            <p>Unfortunately, your application has been rejected for the following reason:</p>
            <blockquote style='color:red;'>{request.Reason}</blockquote>
            <p>Please update your information and try again.</p>
            <p>Best Regards,<br>Asnanii Team</p>";

			await _emailService.SendEmailAsync(userEmail, subject, body);

			return Result<bool>.Success(true, "User rejected and notification email sent.");
		}
	}
}
