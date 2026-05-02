using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetRejectedUsers
{
	public class GetRejectedUsersHandler : IRequestHandler<GetRejectedUsersQuery, Result<List<RejectedUserDto>>>
	{
		private readonly IAdminRepository _adminRepository;

		public GetRejectedUsersHandler(IAdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}

		public async Task<Result<List<RejectedUserDto>>> Handle(GetRejectedUsersQuery request, CancellationToken cancellationToken)
		{
			var rejectedUsers = await _adminRepository.GetRejectedUsersAsync();
			return Result<List<RejectedUserDto>>.Success(rejectedUsers, "Rejected users list retrieved successfully.");
		}
	}
}
