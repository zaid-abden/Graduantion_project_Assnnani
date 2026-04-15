using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Specializations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Queries.GetDoctorsBySpecialization
{
    public class GetDoctorsBySpecializationQuery:IRequest<Result<List<DoctorForPatientDto>>>
    {
        public int  Id { get; set; }
        public GetDoctorsBySpecializationQuery(int Id)
        {
             this.Id = Id;
        }
    }
}
