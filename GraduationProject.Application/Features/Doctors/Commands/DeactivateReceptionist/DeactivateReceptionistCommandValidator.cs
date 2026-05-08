using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.DeactivateReceptionist
{
    public class DeactivateReceptionistCommandValidator:AbstractValidator<DeactivateReceptionistCommand>
    {
        public DeactivateReceptionistCommandValidator()
        {
            RuleFor(x => x.ReceptionistId)
               .GreaterThan(0)
               .WithMessage("Invalid receptionist id");
        }
    }
}
