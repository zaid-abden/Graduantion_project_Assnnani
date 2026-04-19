using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfile
{
	public class GetDoctorProfileValidator : AbstractValidator<GetDoctorProfileQuery>
	{
		public GetDoctorProfileValidator()
		{
			RuleFor(x => x.DoctorId).GreaterThan(0).WithMessage("id is required");
		}
	}
}
