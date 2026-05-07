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
			try
			{
				// 1. جلب البيانات من الـ Repository
				var data = await _adminRepository.GetPatientsOnlyAsync();

				// 2. حالة عدم وجود أي مرضى (قائمة فارغة)
				if (data == null || !data.Any())
				{
					// بنرجع Success لكن مع قائمة فاضية ورسالة واضحة
					return Result<List<PatientListDto>>.Success(new List<PatientListDto>(), "No patients found.");
				}

				// 3. حالة النجاح مع وجود بيانات
				return Result<List<PatientListDto>>.Success(data, "Patients retrieved successfully.");
			}
			catch (Exception ex)
			{
				// 4. حالة حدوث خطأ غير متوقع (Database down, mapping error, etc.)
				// بنستخدم ResultStatus.Failure اللي عندك
				return Result<List<PatientListDto>>.Failure(ResultStatus.Failure, $"An error occurred: {ex.Message}");
			}
		}
	}
}
