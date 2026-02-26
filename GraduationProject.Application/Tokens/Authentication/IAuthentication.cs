using GraduationProject.Application.Tokens.DTOs;
using GraduationProject.Data.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace GraduationProject.Application.Tokens.Authentication
{
    public interface IAthentication
    {
        Task<TokensResult> GetJWTTokenForNewLogin(User identityApp);
        JwtSecurityToken ReadJWTToken(string accesstoken);
        Task<NewTokens> RotationRefandJWT(User user, string? accessToken, string? refreshtoken);
        Task<string> ValidateAccessToken(string accesstoken);
    }
}
