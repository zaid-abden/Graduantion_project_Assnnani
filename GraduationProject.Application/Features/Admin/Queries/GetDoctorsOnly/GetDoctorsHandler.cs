using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetDoctorsOnly
{
	public class GetDoctorsHandler : IRequestHandler<GetDoctorsQuery, Result<List<DoctorListDto>>>
	{
		private readonly IAdminRepository _adminRepository;
		public GetDoctorsHandler(IAdminRepository adminRepository) => _adminRepository = adminRepository;

		public async Task<Result<List<DoctorListDto>>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
		{
			var data = await _adminRepository.GetDoctorsOnlyAsync();
			return Result<List<DoctorListDto>>.Success(data, "Doctors retrieved successfully.");
		}
	}
}
