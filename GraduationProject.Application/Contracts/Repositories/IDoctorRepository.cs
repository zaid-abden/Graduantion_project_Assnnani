using GraduationProject.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IDoctorRepository: IGenericRepository<doctor>
    {
        Task<doctor> GetCurrentDoctor(string userId);
        IQueryable<doctor> GetAll();

    }
}
