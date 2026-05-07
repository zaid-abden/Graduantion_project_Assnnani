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
			try
			{
				// جلب الداتا من الريبوزيتوري
				var data = await _adminRepository.GetStudentsOnlyAsync();

				// حالة عدم وجود طلاب (List فاضية)
				if (data == null || !data.Any())
				{
					return Result<List<StudentListDto>>.Success(new List<StudentListDto>(), "No student doctors found.");
				}

				// حالة النجاح
				return Result<List<StudentListDto>>.Success(data, "Student doctors retrieved successfully.");
			}
			catch (Exception ex)
			{
				// هندلة الأخطاء غير المتوقعة (زي انقطاع الداتابيز مثلاً)
				return Result<List<StudentListDto>>.Failure(ResultStatus.Failure, "An error occurred while fetching student doctors.");
			}
		}
	}
}