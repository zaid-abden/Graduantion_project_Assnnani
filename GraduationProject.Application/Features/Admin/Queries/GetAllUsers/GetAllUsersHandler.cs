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
			// تنظيف البيانات: تحويل المسافات أو النصوص الفارغة لـ NULL
			var sanitizedRequest = request with
			{
				SearchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? null : request.SearchTerm.Trim(),
				Role = string.IsNullOrWhiteSpace(request.Role) ? null : request.Role.Trim(),
				Gender = string.IsNullOrWhiteSpace(request.Gender) ? null : request.Gender.Trim()
			};

			var result = await _adminRepository.GetAllUsersAsync(sanitizedRequest);

			// حتى لو القائمة فاضية (بسبب فلتر Gender مثلاً)، بنرجع Success مع قائمة فاضية
			// ده بيساعد الـ Frontend يعرف إن مفيش Error بس مفيش داتا مطابقة
			if (result.TotalCount == 0)
			{
				return Result<PagedUsersDto>.Success(result, "No users matched your search criteria.");
			}

			return Result<PagedUsersDto>.Success(result, "Users retrieved successfully.");
		}
	}
}