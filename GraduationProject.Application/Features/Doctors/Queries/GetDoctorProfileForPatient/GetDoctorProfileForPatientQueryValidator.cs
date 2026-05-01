using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorProfileForPatient
{
    public class GetDoctorProfileForPatientQueryValidator
      : AbstractValidator<GetDoctorProfileForPatientQuery>
    {
        public GetDoctorProfileForPatientQueryValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .WithMessage("DoctorId must be greater than 0");
        }
    }

}
