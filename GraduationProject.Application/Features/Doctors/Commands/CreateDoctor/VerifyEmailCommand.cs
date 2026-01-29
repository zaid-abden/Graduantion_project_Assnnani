using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Commands.CreateDoctor
{
    public class VerifyEmailCommand:IRequest<Result<string>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
