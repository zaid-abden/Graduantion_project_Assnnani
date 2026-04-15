using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Patients.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Patients.Queries.GetPatientById
{
    public class GetPatientByIdQuery:IRequest<Result<createPatientDto>>
    {
        public int Id { get; set; }
        public GetPatientByIdQuery(int id)
        {
            Id=id;
        }
    }
}
