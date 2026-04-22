using FluentValidation;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorStatistics;

public class GetDoctorStatisticsValidator : AbstractValidator<GetDoctorStatisticsQuery>
{
	public GetDoctorStatisticsValidator()
	{
		RuleFor(x => x.DoctorId)
			.NotEmpty().WithMessage("id is required");
			//.GreaterThan(0).WithMessage("id is wrong");
	}
}