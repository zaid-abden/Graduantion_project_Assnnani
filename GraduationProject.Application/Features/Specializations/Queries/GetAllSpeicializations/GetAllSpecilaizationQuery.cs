using GraduationProject.Application.Features.Specializations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetAllSpeicializations
{
    public class GetAllSpecilaizationQuery:IRequest<List<SpecializationDto>>
    {
    }
}
