using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GraduationProject.Application.Features.Admin.Queries.GetDoctorsByStatus;

public class GetDoctorsByStatusHandler : IRequestHandler<GetDoctorsByStatusQuery, Result<List<DoctorStatusDto>>>
{
	private readonly IAdminRepository _adminRepository;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public GetDoctorsByStatusHandler(IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
	{
		_adminRepository = adminRepository;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<Result<List<DoctorStatusDto>>> Handle(GetDoctorsByStatusQuery request, CancellationToken cancellationToken)
	{
		// 1. هات الـ Entities من الريبو
		var doctors = await _adminRepository.GetDoctorsByStatusAsync(request.Status);

		var httpRequest = _httpContextAccessor.HttpContext.Request;
		var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";

		// 2. حولهم لـ DTOs هنا (Manual Mapping)
		var result = doctors.Select(d => new DoctorStatusDto(
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
			d.Languages != null ? string.Join(", ", d.Languages) : null,
			d.price, // السعر من الـ Entity
			!string.IsNullOrEmpty(d.User?.ImageUrl) ? $"{baseUrl}/{d.User.ImageUrl.TrimStart('/')}" : $"{baseUrl}/images/default-user.png",
			!string.IsNullOrEmpty(d.DoctorCertificate) ? $"{baseUrl}/{d.DoctorCertificate.TrimStart('/')}" : "",
			d.User?.CreatedAt ?? DateTime.Now
		)).ToList();

		return Result<List<DoctorStatusDto>>.Success(result, message: "Doctors retrieved successfully.");
	}
}