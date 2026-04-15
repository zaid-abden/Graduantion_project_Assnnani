using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Identity
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
       
        IEnumerable<string> Roles { get; }
      


    }
}
