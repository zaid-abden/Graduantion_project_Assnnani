using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Allergies.Commands.AddAllergy
{
    public class AddAllergyCommand:IRequest<Result<int>>
    {
       public string Name { get; set; }
    }
}
