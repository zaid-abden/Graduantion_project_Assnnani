using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.Commands.RejectUser;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.VerifyUser
{
	public class VerifyUserHandler : IRequestHandler<VerifyUserCommand, Result<bool>>
	{
		private readonly IAdminRepository _adminRepository;
		private readonly IEmailService _emailService;

		public VerifyUserHandler(IAdminRepository adminRepository, IEmailService emailService)
		{
			_adminRepository = adminRepository;
			_emailService = emailService;
		}

		public async Task<Result<bool>> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
		{
			// 1. تحديث قاعدة البيانات وجلب إيميل المستخدم
			var userEmail = await _adminRepository.ApproveUserAsync(request.Id);

			if (string.IsNullOrEmpty(userEmail))
			{
				return Result<bool>.Failure(ResultStatus.NotFound, "User not found or already verified.");
			}

			// 2. إرسال إيميل القبول
			string subject = "Welcome to Asnanii - Account Verified!";
			string body = $@"
            <h3>Congratulations!</h3>
            <p>Your account on <b>Asnanii Clinic Management System</b> has been verified.</p>
            <p>You can now log in and start managing your clinic and using our AI features.</p>
            {(string.IsNullOrEmpty(request.Note) ? "" : $"<p><b>Admin Note:</b> {request.Note}</p>")}
            <p>Best Regards,<br>Asnanii Team</p>";

			await _emailService.SendEmailAsync(userEmail, subject, body);

			return Result<bool>.Success(true, "User verified successfully and notification email sent.");
		}
	}
}
