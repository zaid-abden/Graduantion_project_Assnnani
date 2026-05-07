using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GraduationProject.Application.Features.Admin.Queries.GetRejectedUsers;

public class GetRejectedUsersHandler : IRequestHandler<GetRejectedUsersQuery, Result<List<RejectedDoctorDetailsDto>>>
{
	private readonly IAdminRepository _adminRepository;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public GetRejectedUsersHandler(IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
	{
		_adminRepository = adminRepository;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<Result<List<RejectedDoctorDetailsDto>>> Handle(GetRejectedUsersQuery request, CancellationToken cancellationToken)
	{
		var doctors = await _adminRepository.GetRejectedDoctorsOnlyAsync();

		var httpRequest = _httpContextAccessor.HttpContext.Request;
		var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";

		var result = doctors.Select(d => new RejectedDoctorDetailsDto(
			d.DoctorId.ToString(),
			$"{d.User?.FirstName} {d.User?.LastName}".Trim(),
			d.User?.Email ?? "",
			d.User?.PhoneNumber ?? "",
			d.User?.Gender ?? "",
			d.User?.BirthDate,
			d.MedicalLicenseNumber,
			d.RejectionReason ?? "No reason provided", // سبب الرفض
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
			d.price,
			!string.IsNullOrEmpty(d.User?.ImageUrl) ? $"{baseUrl}/{d.User.ImageUrl.TrimStart('/')}" : $"{baseUrl}/images/default-user.png",
			!string.IsNullOrEmpty(d.DoctorCertificate) ? $"{baseUrl}/{d.DoctorCertificate.TrimStart('/')}" : "",
			d.VerifiedAt // نعتبر تاريخ التحقق هو تاريخ الرفض في حالتنا
		)).ToList();

		return Result<List<RejectedDoctorDetailsDto>>.Success(result, message: "Rejected doctors list retrieved successfully.");
	}
}