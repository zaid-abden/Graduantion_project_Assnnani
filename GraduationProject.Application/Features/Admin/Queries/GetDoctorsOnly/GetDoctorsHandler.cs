using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GraduationProject.Application.Features.Admin.Queries.GetDoctorsOnly
{
	public class GetDoctorsHandler : IRequestHandler<GetDoctorsQuery, Result<List<DoctorOnlyDetailsDto>>>
	{
		private readonly IAdminRepository _adminRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public GetDoctorsHandler(IAdminRepository adminRepository, IHttpContextAccessor httpContextAccessor)
		{
			_adminRepository = adminRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<Result<List<DoctorOnlyDetailsDto>>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
		{
			var doctors = await _adminRepository.GetDoctorsOnlyAsync();

			var httpRequest = _httpContextAccessor.HttpContext.Request;
			var baseUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}";

			var result = doctors.Select(d => new DoctorOnlyDetailsDto(
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
				d.price, // السعر سمول
				!string.IsNullOrEmpty(d.User?.ImageUrl) ? $"{baseUrl}/{d.User.ImageUrl.TrimStart('/')}" : $"{baseUrl}/images/default-user.png",
				!string.IsNullOrEmpty(d.DoctorCertificate) ? $"{baseUrl}/{d.DoctorCertificate.TrimStart('/')}" : "",
				d.User?.CreatedAt ?? DateTime.Now
			)).ToList();

			return Result<List<DoctorOnlyDetailsDto>>.Success(result, "Doctors retrieved successfully.");
		}
	}
}
