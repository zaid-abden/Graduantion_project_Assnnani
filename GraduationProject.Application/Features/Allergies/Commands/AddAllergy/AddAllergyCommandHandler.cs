using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Allergies.Commands.AddAllergy
{
    public class AddAllergyCommandHandler : IRequestHandler<AddAllergyCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public AddAllergyCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(AddAllergyCommand request, CancellationToken cancellationToken)
        {
            var existing = await unitOfWork.Allergies.Query()
                .AnyAsync(x => x.Name == request.Name, cancellationToken);
            if(existing)
                return Result<int>.Failure(ResultStatus.Conflict, "Allergy with the same name already exists");
            var allergy = new Allergy
            {
                Name = request.Name
            };
            await unitOfWork.Allergies.AddAsync(allergy);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(allergy.Id);
        }
    }
}
