using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Specializations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetSpecializationById
{
    public class GetSpecializationByIdQuery: IRequest<Result<SpecializationDto>>
    {
        public int Id { get; set; }
        public GetSpecializationByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
