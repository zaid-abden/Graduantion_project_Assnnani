using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using GraduationProject.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Repositories
{
    public class ScheduleSlotRepository : GenericRepository<ScheduleSlot>, IScheduleSlotRepository
    {
        public ScheduleSlotRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
