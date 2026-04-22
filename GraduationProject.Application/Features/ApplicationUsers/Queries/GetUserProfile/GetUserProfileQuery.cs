using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.ApplicationUsers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.ApplicationUsers.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<Result<ApplicationUserDto>>
    {
    }
}
