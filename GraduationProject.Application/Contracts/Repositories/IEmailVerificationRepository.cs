using GraduationProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Repositories
{
    public interface IEmailVerificationRepository
    {
        Task AddVerification(EmailVerification emailVerification);
       public  Task<List<EmailVerification>> GetVerifications();  
    }
}
