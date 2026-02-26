using GraduationProject.Data.Models;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<UserRefreshToken>
    {
        Task<UserRefreshToken> GetRefreshTokenAsync(string UserId, string JTI, string RefreshToeken);
    }
}
