using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Auth.Commands.UnlockUser
{
    public class UnlockUserCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public UnlockUserCommand(string id)
        {
            UserId = id;    
        }
    }
}
