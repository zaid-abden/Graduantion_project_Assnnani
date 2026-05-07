using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetDoctorsByStatus
{
	public class GetDoctorsByStatusHandler : IRequestHandler<GetDoctorsByStatusQuery, Result<List<DoctorStatusDto>>>
	{
		private readonly IAdminRepository _adminRepository;

		public GetDoctorsByStatusHandler(IAdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}

		public async Task<Result<List<DoctorStatusDto>>> Handle(GetDoctorsByStatusQuery request, CancellationToken cancellationToken)
		{
			var doctors = await _adminRepository.GetDoctorsByStatusAsync(request.Status);
			return Result<List<DoctorStatusDto>>.Success(doctors);
		}
	}
}
