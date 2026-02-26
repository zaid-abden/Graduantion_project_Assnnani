using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Infrastructure.Repositories
{
    internal class RefreshTokenRepository : GenericRepository<UserRefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRefreshToken> GetRefreshTokenAsync(string UserId, string JTI, string RefreshToeken)
        {
            return await _dbContext.UserRefreshTokens
                .FirstOrDefaultAsync(ww => ww.JTI == JTI && ww.UserId == UserId && ww.RefreshToken == RefreshToeken);
        }
    }
}
