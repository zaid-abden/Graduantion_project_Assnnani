using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Tokens.DTOs;
using GraduationProject.Data.Identity;
using GraduationProject.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GraduationProject.Application.Tokens.Authentication
{
    internal class Athentication : IAthentication
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUnitOfWork _refreshTokenRepo;
        private readonly UserManager<User> _userManager;

        public Athentication(IOptions<JwtSettings> options,
           IUnitOfWork refreshTokenRepo,
           UserManager<User> userManager)
        {
            _jwtSettings = options.Value;
            _refreshTokenRepo = refreshTokenRepo;
            _userManager = userManager;
            //_userrefreshtoken = new ConcurrentDictionary<string, RefreshToken>();
        }
        public async Task<TokensResult> GetJWTTokenForNewLogin(User user)
        {
            var (jwttoken, accessToken) = await GenerateJWTToken(user);

            var RefreshToken = NewRefreshToken(user.UserName);

            var userRefreshToken = new UserRefreshToken
            {
                Created = DateTime.UtcNow,
                ExpiryDate = RefreshToken.Expiration,
                IsRevoked = false,
                IsUsed = false,
                JTI = jwttoken.Id,
                RefreshToken = RefreshToken.RefToken,
                UserId = user.Id
            };

            await _refreshTokenRepo.RefreshTokenRepository.AddAsync(userRefreshToken);

            var result = new TokensResult
            {
                Token = accessToken,
                refreshToken = RefreshToken
            };

            return result;
        }
        private async Task<TokensResult> GetJWTTokenForLogindUsers(User? user, DateTime? Expiration)
        {
            var (jwttoken, accessToken) = await GenerateJWTToken(user);

            var RefreshToken = NewRefreshToken(user.UserName);

            var userRefreshToken = new UserRefreshToken
            {
                Created = DateTime.UtcNow,
                ExpiryDate = Expiration.Value,
                IsRevoked = false,
                IsUsed = false,
                JTI = jwttoken.Id,
                RefreshToken = RefreshToken.RefToken,
                UserId = user.Id
            };

            await _refreshTokenRepo.RefreshTokenRepository.AddAsync(userRefreshToken);

            var result = new TokensResult
            {
                Token = accessToken,
                refreshToken = RefreshToken
            };

            return result;
        }

        public JwtSecurityToken ReadJWTToken(string accesstoken)
        {
            if (string.IsNullOrEmpty(accesstoken))
            {
                throw new ArgumentNullException(nameof(accesstoken));
            }
            return new JwtSecurityTokenHandler().ReadJwtToken(accesstoken);
        }

        public async Task<string> ValidateAccessToken(string accesstoken)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = false,
            };

            var handler = new JwtSecurityTokenHandler();
            var result = await handler.ValidateTokenAsync(accesstoken, parameters);

            if (result == null)
                throw new Exception("Invalid Token");

            return "Verified";
        }

        public async Task<NewTokens> RotationRefandJWT(User user, string? accessToken, string? refreshtoken)
        {
            var NewToken = new NewTokens();
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException(nameof(accessToken));
            if (string.IsNullOrEmpty(refreshtoken))
                throw new ArgumentNullException(nameof(refreshtoken));
            var JWToken = ReadJWTToken(accessToken);
            if (JWToken.ValidTo > DateTime.UtcNow)
                return NewToken.Errors("Token not Expired");

            var userClaim = JWToken.Claims.FirstOrDefault(c => c.Type == "id");
            if (userClaim == null)
                return NewToken.Errors("userid is Not Found");
            string userid = userClaim.Value;

            var jtiClaim = JWToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            if (jtiClaim == null)
                return NewToken.Errors("JTI is Not Found");
            string JTI = jtiClaim.Value;

            var RefTokenData = await _refreshTokenRepo
                .RefreshTokenRepository
                .GetRefreshTokenAsync(userid, JTI, refreshtoken);
            if (RefTokenData is null)
                return NewToken.Errors("RefToken NotFound");
            var Expiration = RefTokenData.ExpiryDate;
            if (Expiration <= DateTime.UtcNow)
            {
                return NewToken.Errors("Expiratton RefToken");
            }
            if (RefTokenData.IsRevoked)
            {
                return NewToken.Errors("RefToken Revoked");
            }
            if (RefTokenData.IsUsed)
            {
                return NewToken.Errors("RefToken IsUsed");
            }
            var GenerateNewAccessToken = await GetJWTTokenForLogindUsers(user, Expiration);
            RefTokenData.IsUsed = true;
            _refreshTokenRepo.RefreshTokenRepository.Update(RefTokenData);
            return NewToken.GetNewTokens(GenerateNewAccessToken);
        }

        private async Task<(JwtSecurityToken, string)> GenerateJWTToken(User user)
        {

            var claims = await GetClaims(user);

            var jwttoken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256Signature)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwttoken);
            return (jwttoken, accessToken);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>()
            {
               new Claim(ClaimTypes.NameIdentifier,user.UserName),
               new Claim(ClaimTypes.Email,user.Email),
               new Claim(nameof(userclaims.PhoneNumber),user.PhoneNumber),
               new Claim(nameof(userclaims.id),user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var herclaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(herclaims);
            return claims;
        }
        private RefreshToken NewRefreshToken(string UserName)
        {
            var RefreshToken = new RefreshToken
            {
                Expiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                UserName = UserName,
                RefToken = GenerateRefeshToken()

            };
            return RefreshToken;
        }

        private string GenerateRefeshToken()
        {
            var RandomNumber = new byte[32];
            var RandomGenerator = RandomNumberGenerator.Create();
            RandomGenerator.GetBytes(RandomNumber);
            return Convert.ToBase64String(RandomNumber);
        }
    }
}