using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Commands.UpdateSpecialization
{
    public class UpdateSpecializationCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
