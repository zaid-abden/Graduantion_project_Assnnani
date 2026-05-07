namespace GraduationProject.Application.Features.Admin.DTOs;

public record DoctorOnlyDetailsDto(
	string DoctorId,
	string FullName,
	string Email,
	string PhoneNumber,
	string Gender,
	DateTime? BirthDate,
	string MedicalLicenseNumber,
	string? ClinicName,
	string? ClinicLocation,
	string? ClinicPhoneNumber,
	string? About,
	int YearsOfExperience,
	string Country,
	string City,
	string Street,
	string Details,
	int Degree,
	string? Education,
	string? Languages,
	decimal? Price,
	string FullProfileImageUrl,
	string FullCertificateUrl,
	DateTime CreatedAt
);