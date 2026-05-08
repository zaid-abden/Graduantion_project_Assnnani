using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetPatientMedicalHistory
{
    public class GetPatientMedicalHistoryQueryValidator
        : AbstractValidator<GetPatientMedicalHistoryQuery>
    {
        public GetPatientMedicalHistoryQueryValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required")
                .GreaterThan(0).WithMessage("PatientId must be greater than 0");
        }
    }
}
