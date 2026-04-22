using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Specializations.Commands.DeleteSpecialization
{
    public class DeleteSpecializationCommand:IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DeleteSpecializationCommand(int Id)
        {
            this.Id = Id;
        }
    }
}
