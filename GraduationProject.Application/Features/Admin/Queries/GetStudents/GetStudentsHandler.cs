using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetStudents
{
	public class GetStudentsHandler : IRequestHandler<GetStudentsQuery, Result<List<StudentListDto>>>
	{
		private readonly IAdminRepository _adminRepository;
		public GetStudentsHandler(IAdminRepository adminRepository) => _adminRepository = adminRepository;

		public async Task<Result<List<StudentListDto>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
		{
			var data = await _adminRepository.GetStudentsOnlyAsync();
			return Result<List<StudentListDto>>.Success(data, "Student doctors retrieved successfully.");
		}
	}
}
