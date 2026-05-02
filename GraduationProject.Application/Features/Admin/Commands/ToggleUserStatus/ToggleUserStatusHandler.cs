using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Commands.ToggleUserStatus
{
	public class ToggleUserStatusHandler : IRequestHandler<ToggleUserStatusCommand, Result<bool>>
	{
		private readonly IAdminRepository _adminRepository;

		public ToggleUserStatusHandler(IAdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}

		public async Task<Result<bool>> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
		{
			var result = await _adminRepository.ToggleUserStatusAsync(request.UserId);

			if (!result)
			{
				// نمرر الـ Status أولاً ثم الـ Error كما يطلب كلاس الـ Result عندك
				return Result<bool>.Failure(ResultStatus.NotFound, "User not found or operation failed.");
			}

			return Result<bool>.Success(true, "User status updated successfully.");
		}
	}
}
