using FluentValidation;
using GraduationProject.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Http;

//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Commands.UpdateSpecialization
{
    public class UpdateSpecializationCommandValidator
     : AbstractValidator<UpdateSpecializationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdateSpecializationCommandValidator(IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
          
            RuleFor(x => x)
                .NotNull().WithMessage("Command cannot be null");

            // Validate Id
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Specialization ID must be a positive number");

           
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Specialization name cannot be null")
                .NotEmpty().WithMessage("Specialization name is required")
                .MaximumLength(100).WithMessage("Specialization name must not exceed 100 characters")
                .Must(ckeckExist).WithMessage("Specialization name already exists");
        }

        private bool ckeckExist(string name)
        {
            return !_unitOfWork.Specialization.IsSpecializationNameExist(name);
        }

    }
}
