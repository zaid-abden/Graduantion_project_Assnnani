using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetPendingUsers
{
	public class GetPendingUsersHandler : IRequestHandler<GetPendingUsersQuery, Result<List<PendingUserDto>>>
	{
		private readonly IAdminRepository _adminRepository;

		public GetPendingUsersHandler(IAdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}

		public async Task<Result<List<PendingUserDto>>> Handle(GetPendingUsersQuery request, CancellationToken cancellationToken)
		{
			var users = await _adminRepository.GetPendingUsersAsync();

			return Result<List<PendingUserDto>>.Success(users, "Pending users list retrieved successfully.");
		}
	}
}
