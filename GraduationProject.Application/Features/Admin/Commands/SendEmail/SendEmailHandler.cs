using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.SendEmail
{
	public class SendEmailHandler : IRequestHandler<SendEmailCommand, Result<bool>>
	{
		private readonly IAdminRepository _adminRepository;
		private readonly IEmailService _emailService;

		public SendEmailHandler(IAdminRepository adminRepository, IEmailService emailService)
		{
			_adminRepository = adminRepository;
			_emailService = emailService;
		}

		public async Task<Result<bool>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
		{
			var emails = await _adminRepository.GetEmailsByTargetAsync(request.UserId, request.RoleName, request.UserIds);

			if (emails == null || !emails.Any())
			{
				// الالتزام بـ Signature الميثود: (ResultStatus, string)
				return Result<bool>.Failure(ResultStatus.Failure, "No valid recipients found.");
			}

			foreach (var email in emails)
			{
				await _emailService.SendEmailAsync(email, request.Subject, request.Body);
			}

			return Result<bool>.Success(true, $"Email sent successfully to {emails.Count} recipients.");
		}
	}
}
