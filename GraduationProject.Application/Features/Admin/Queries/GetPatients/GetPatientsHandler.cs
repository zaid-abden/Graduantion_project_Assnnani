using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetPatients
{
	public class GetPatientsHandler : IRequestHandler<GetPatientsQuery, Result<List<PatientListDto>>>
	{
		private readonly IAdminRepository _adminRepository;
		public GetPatientsHandler(IAdminRepository adminRepository) => _adminRepository = adminRepository;

		public async Task<Result<List<PatientListDto>>> Handle(GetPatientsQuery request, CancellationToken cancellationToken)
		{
			var data = await _adminRepository.GetPatientsOnlyAsync();
			return Result<List<PatientListDto>>.Success(data, "Patients retrieved successfully.");
		}
	}
}
