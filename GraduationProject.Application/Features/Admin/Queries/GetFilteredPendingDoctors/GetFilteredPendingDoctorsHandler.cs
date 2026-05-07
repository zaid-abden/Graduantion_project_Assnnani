using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GraduationProject.Application.Features.Admin.Queries.GetFilteredPendingDoctors
{
	public class FilterPendingDoctorsHandler : IRequestHandler<FilterPendingDoctorsQuery, Result<FilterPendingDoctorsResultDto>>
	{
		private readonly IAdminRepository _adminRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public FilterPendingDoctorsHandler(IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
		{
			_adminRepository = adminRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<Result<FilterPendingDoctorsResultDto>> Handle(FilterPendingDoctorsQuery request, CancellationToken cancellationToken)
		{
			var (doctors, totalCount) = await _adminRepository.FilterPendingDoctorsAsync(
				request.SearchTerm, request.PageNumber, request.PageSize);

			var httpRequest = _httpContextAccessor.HttpContext.Request;
			var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";

			var doctorDetailsList = doctors.Select(d =>
			{
				string? languagesJoined = d.Languages != null ? string.Join(", ", d.Languages) : null;

				var fullProfilePath = !string.IsNullOrEmpty(d.User?.ImageUrl)
					? $"{baseUrl}/{d.User.ImageUrl.TrimStart('/')}"
					: $"{baseUrl}/images/default-user.png";

				var fullCertificatePath = !string.IsNullOrEmpty(d.DoctorCertificate)
					? $"{baseUrl}/{d.DoctorCertificate.TrimStart('/')}"
					: "";

				return new FilterPendingDoctorDetailsDto(
					d.DoctorId.ToString(),
					$"{d.User?.FirstName} {d.User?.LastName}".Trim(),
					d.User?.Email ?? "",
					d.User?.PhoneNumber ?? "",
					d.User?.Gender ?? "",
					d.User?.BirthDate,
					d.MedicalLicenseNumber,
					d.ClinicName,
					d.ClinicLocation,
					d.ClinicPhoneNumber,
					d.About,
					d.YearsOfExperience,
					d.Country,
					d.City,
					d.Street,
					d.Details,
					(int)d.Degree,
					d.Education,
					languagesJoined,
					d.price, // لاحظ price سمول كما في الـ Entity بتاعك
					fullProfilePath,
					fullCertificatePath,
					d.User?.CreatedAt ?? DateTime.Now
				);
			}).ToList();

			var resultData = new FilterPendingDoctorsResultDto(doctorDetailsList, totalCount);

			return Result<FilterPendingDoctorsResultDto>.Success(resultData, message: "Pending doctors filtered successfully.");
		}
	}
}
