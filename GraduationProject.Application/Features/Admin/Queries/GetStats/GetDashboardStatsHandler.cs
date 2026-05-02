using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetStats
{
	public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
	{
		private readonly IAdminRepository _adminRepository;

		public GetDashboardStatsHandler(IAdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}

		public async Task<Result<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
		{
			var stats = await _adminRepository.GetDashboardStatsAsync();

			return Result<DashboardStatsDto>.Success(stats, "Dashboard statistics retrieved successfully.");
		}
	}
}
