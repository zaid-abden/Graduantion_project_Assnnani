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
			try
			{
				var data = await _adminRepository.GetDoctorsOnlyAsync();

				if (data == null || !data.Any())
				{
					return Result<List<DoctorListDto>>.Success(new List<DoctorListDto>(), "No doctors found.");
				}

				return Result<List<DoctorListDto>>.Success(data, "Doctors retrieved successfully.");
			}
			catch (Exception ex)
			{
				return Result<List<DoctorListDto>>.Failure(ResultStatus.Failure, "An error occurred while fetching doctors.");
			}
		}
	}
}
