using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetSchedulesByDoctor_Day
{
    public class GetSchedulesByDoctorDayQueryValidator
    : AbstractValidator<GetSchedulesByDoctorDayQuery>
    {
        public GetSchedulesByDoctorDayQueryValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0);

            RuleFor(x => x.Day)
                .IsInEnum();
        }
    }

}
