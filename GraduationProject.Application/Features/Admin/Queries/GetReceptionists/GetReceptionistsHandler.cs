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
			try
			{
				var data = await _adminRepository.GetReceptionistsOnlyAsync();

				if (data == null || !data.Any())
				{
					return Result<List<ReceptionistListDto>>.Success(new List<ReceptionistListDto>(), "No receptionists found.");
				}

				return Result<List<ReceptionistListDto>>.Success(data, "Receptionists retrieved successfully.");
			}
			catch (Exception ex)
			{
				// هندلة أي خطأ في الـ Database أو الـ Mapping
				return Result<List<ReceptionistListDto>>.Failure(ResultStatus.Failure, "An error occurred while fetching receptionists.");
			}
		}
	}
}