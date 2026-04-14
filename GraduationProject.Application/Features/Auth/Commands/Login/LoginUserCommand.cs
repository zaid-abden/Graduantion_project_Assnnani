using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Auth.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Auth.Commands.Login
{
    public class LoginUserCommand:IRequest<Result<AuthDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
