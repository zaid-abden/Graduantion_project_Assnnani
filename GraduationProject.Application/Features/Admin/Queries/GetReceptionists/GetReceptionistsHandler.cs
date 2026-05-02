using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetReceptionists
{
	public class GetReceptionistsHandler : IRequestHandler<GetReceptionistsQuery, Result<List<ReceptionistListDto>>>
	{
		private readonly IAdminRepository _adminRepository;
		public GetReceptionistsHandler(IAdminRepository adminRepository) => _adminRepository = adminRepository;

		public async Task<Result<List<ReceptionistListDto>>> Handle(GetReceptionistsQuery request, CancellationToken cancellationToken)
		{
			var data = await _adminRepository.GetReceptionistsOnlyAsync();
			return Result<List<ReceptionistListDto>>.Success(data);
		}
	}
}
