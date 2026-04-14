using FluentValidation;
using GraduationProject.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Commands.CreateSpecialization
{
    public class CreateSpecializationCommandValidator
    : AbstractValidator<CreateSpecializationCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreateSpecializationCommandValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(x => x.Name)
             .Cascade(CascadeMode.Stop)
             .NotNull().WithMessage("Specialization name cannot be null")
             .NotEmpty().WithMessage("Specialization name is required")
             
             .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
             .Must(ckeckExist)
             .WithMessage("Specialization name already exists");
        }
        private bool ckeckExist(string name)
        {
            return ! unitOfWork.Specialization.IsSpecializationNameExist(name);
        }
    }
}
