using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Specializations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetSpecializationById
{
    public class GetSpecializationByIdQueryHandler : IRequestHandler<GetSpecializationByIdQuery, Result<SpecializationDto>>
        
    {
        private readonly IUnitOfWork unitOfWork;

        public GetSpecializationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<SpecializationDto>> Handle(GetSpecializationByIdQuery request, CancellationToken cancellationToken)
        {
            var specilaization = await unitOfWork.Specialization.GetByIdAsync(request.Id);
            if (specilaization == null)
                return Result<SpecializationDto>.Failure(ResultStatus.NotFound, "Specialization not found");
            var specializationDto = new SpecializationDto
            {
                Id = specilaization.Id,
                Name = specilaization.Name,
                
            };
            return Result<SpecializationDto>.Success(specializationDto);
        }
    }
}
