using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetAllUsers
{
	public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<PagedUsersDto>>
	{
		private readonly IAdminRepository _adminRepository;

		public GetAllUsersHandler(IAdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}

		public async Task<Result<PagedUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
		{
			var result = await _adminRepository.GetAllUsersAsync(request);
			return Result<PagedUsersDto>.Success(result, "Users retrieved successfully.");
		}
	}
}
