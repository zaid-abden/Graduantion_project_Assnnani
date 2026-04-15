using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllActiveScheduleForDoctor
{
    public class GetAllActiveScheduleForDoctorQueryValidator
        : AbstractValidator<GetAllActiveScheduleForDoctorQuery>
    {
        public GetAllActiveScheduleForDoctorQueryValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .WithMessage("DoctorId must be greater than zero.");
        }
    }
}
