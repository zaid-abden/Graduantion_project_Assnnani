using GraduationProject.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace GraduationProject.Application.Features.Doctors.Commands.UpdateDoctorProfile
{
	public class UpdateDoctorProfileCommand : IRequest<Result<bool>>
	{
		public int DoctorId { get; set; } 
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string About { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string Country { get; set; }
		public int YearsOfExperience { get; set; }
	}
}
