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

    public class DoctorRepository
        : GenericRepository<doctor>, IDoctorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DoctorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<doctor> GetAll()
        {
           return _dbContext.Doctors.AsQueryable();
        }

        public async Task<doctor> GetCurrentDoctor(string userId)
        {
            var doctor=await _dbContext.Doctors.Include(x=>x.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);
            return doctor!;
        }
    }
}
