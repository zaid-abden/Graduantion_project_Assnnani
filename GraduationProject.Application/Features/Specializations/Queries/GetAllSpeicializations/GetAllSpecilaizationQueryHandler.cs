using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Specializations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetAllSpeicializations
{
    public class GetAllSpecilaizationQueryHandler : IRequestHandler<GetAllSpecilaizationQuery, List<SpecializationDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllSpecilaizationQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<SpecializationDto>> Handle(GetAllSpecilaizationQuery request, CancellationToken cancellationToken)
        {
            var specializations = await unitOfWork.Specialization.GetAllAsync();
            var specializationDtos = specializations.Select(s => new SpecializationDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
            return specializationDtos;
        }
    }
}
