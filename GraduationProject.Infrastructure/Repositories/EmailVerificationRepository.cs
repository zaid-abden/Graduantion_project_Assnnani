using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Repositories
{
    public class EmailVerificationRepository:IEmailVerificationRepository
    {
        private readonly ApplicationDbContext _context;
        public EmailVerificationRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public async Task AddVerification(EmailVerification emailVerification)
        {
          await  _context.EmailVerifications.AddAsync(emailVerification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmailVerification>> GetVerifications()
        {
            return await _context.EmailVerifications.ToListAsync();
        }
    }
}
