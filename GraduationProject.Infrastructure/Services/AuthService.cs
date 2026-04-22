using GraduationProject.Application.Contracts.Services;
using GraduationProject.Application.Settings;
using GraduationProject.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly JwtSetting jwtSetting;
        private readonly RoleManager<IdentityRole> roleManager;
        public AuthService(UserManager<User> userManager,IOptions<JwtSetting> options, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.jwtSetting = options.Value;    
            this.roleManager = roleManager;
        }
        public async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email??""),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName??""),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),

        
            };

            var userCliams = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var roleName = await roleManager.FindByNameAsync(role);
                var roleCliams = await roleManager.GetClaimsAsync(roleName!);
                foreach (var claim in roleCliams)
                {
                    userCliams.Add(claim);
                }

            }
            claims.AddRange(userCliams);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                signingCredentials: cred,
                expires: DateTime.UtcNow.AddMinutes(jwtSetting.DurationInMinutes)
                );
            return new JwtSecurityTokenHandler().WriteToken(token); ;
        }
    }
}
