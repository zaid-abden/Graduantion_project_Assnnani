using GraduationProject.Application.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string? Email => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public IEnumerable<string> Roles => httpContextAccessor.HttpContext?.User?.Claims
           .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();
     
      
    }
  
}
