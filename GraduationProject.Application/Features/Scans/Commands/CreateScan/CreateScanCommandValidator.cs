using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.CreateScan
{
    public class CreateScanCommandValidator : AbstractValidator<CreateScanCommand>
    {
        public CreateScanCommandValidator()
        {
            
            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .WithMessage("PatientId must be greater than 0.");

         
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.")
                .Must(BeValidFile)
                .WithMessage("Invalid file type. Only JPG, PNG, and PDF are allowed.")
                .Must(BeValidSize)
                .WithMessage("File size must not exceed 5 MB.");
        }

        private bool BeValidFile(IFormFile file)
        {
            if (file == null) return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(extension);
        }

        private bool BeValidSize(IFormFile file)
        {
            if (file == null) return false;

            const long maxSize = 5 * 1024 * 1024; 
            return file.Length <= maxSize;
        }
    }
}
