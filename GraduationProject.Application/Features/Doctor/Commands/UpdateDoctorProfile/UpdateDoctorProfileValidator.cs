using FluentValidation;

namespace GraduationProject.Application.Features.Doctors.Commands.UpdateDoctorProfile
{
	public class UpdateDoctorProfileValidator : AbstractValidator<UpdateDoctorProfileCommand>
	{
		public UpdateDoctorProfileValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
			RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Phone number is required.")
				.Matches(@"^\d{11}$").WithMessage("Phone number must be exactly 11 digits.");
			RuleFor(x => x.YearsOfExperience)
				.InclusiveBetween(0, 50).WithMessage("Years of experience must be between 0 and 50.");
			RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
		}
	}
}
