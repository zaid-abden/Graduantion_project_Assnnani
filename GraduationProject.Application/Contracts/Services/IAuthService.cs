using GraduationProject.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Services
{
    public interface IAuthService
    {
        public Task<string> GenerateToken(User user);
    }
}
