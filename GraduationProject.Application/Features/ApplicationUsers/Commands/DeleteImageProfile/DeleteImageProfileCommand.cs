using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.ApplicationUsers.Commands.DeleteImageProfile
{
    public class DeleteImageProfileCommand : IRequest<Result<string>>
    {
    }
}
