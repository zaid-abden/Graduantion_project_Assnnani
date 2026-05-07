using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GraduationProject.Application.Features.Admin.Queries.GetPendingDoctorById
{
	public class GetPendingDoctorByIdHandler : IRequestHandler<GetPendingDoctorByIdQuery, Result<PendingDoctorDetailsDto>>
	{
		private readonly IAdminRepository _adminRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public GetPendingDoctorByIdHandler(IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
		{
			_adminRepository = adminRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<Result<PendingDoctorDetailsDto>> Handle(GetPendingDoctorByIdQuery request, CancellationToken cancellationToken)
		{
			// جلب بيانات الدكتور
			var doctor = await _adminRepository.GetPendingDoctorByIdAsync(request.DoctorId);

			if (doctor == null)
			{
				return Result<PendingDoctorDetailsDto>.Failure(ResultStatus.NotFound, "Doctor not found or not pending.");
			}

			// 1. معالجة الروابط الكاملة للصور
			var httpRequest = _httpContextAccessor.HttpContext.Request;
			var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";

			// استخدام ImageUrl من جدول الدكتور أو اليوزر حسب سياق رفع الصور عندك
			var fullProfilePath = !string.IsNullOrEmpty(doctor.User?.ImageUrl)
				? $"{baseUrl}/{doctor.User.ImageUrl.TrimStart('/')}"
				: $"{baseUrl}/images/default-doctor.png";

			var fullCertificatePath = !string.IsNullOrEmpty(doctor.DoctorCertificate)
				? $"{baseUrl}/{doctor.DoctorCertificate.TrimStart('/')}"
				: null;

			// 2. معالجة الـ Languages (تحويل List<string> إلى string مفصل بفاصلة)
			string? languagesJoined = doctor.Languages != null
				? string.Join(", ", doctor.Languages)
				: null;

			// 3. الـ Manual Mapping الدقيق
			// لاحظ استخدام doctor.price (سمول) كما في الـ Entity بتاعك
			var dto = new PendingDoctorDetailsDto(
				doctor.DoctorId.ToString(),
				$"{doctor.User?.FirstName} {doctor.User?.LastName}".Trim(),
				doctor.User?.Email ?? "",
				doctor.User?.PhoneNumber ?? "",
				doctor.User?.Gender ?? "",
				doctor.User?.BirthDate,
				doctor.MedicalLicenseNumber,
				doctor.ClinicName,
				doctor.ClinicLocation,
				doctor.ClinicPhoneNumber,
				doctor.About,
				doctor.YearsOfExperience,
				doctor.Country,
				doctor.City,
				doctor.Street,
				doctor.Details,
				(int)doctor.Degree,
				doctor.Education,
				languagesJoined,
				doctor.price, // تم التعديل ليتوافق مع الـ Entity (سمول)
				fullProfilePath,
				fullCertificatePath ?? "",
				doctor.User?.CreatedAt ?? DateTime.Now
			);

			return Result<PendingDoctorDetailsDto>.Success(dto, "Doctor details retrieved successfully.");
		}
	}
}
