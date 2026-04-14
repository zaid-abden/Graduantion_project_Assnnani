using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Application.Settings;
using GraduationProject.Data.Identity;
using GraduationProject.Infrastructure.Context;
using GraduationProject.Infrastructure.Extensions;
using GraduationProject.Infrastructure.Identity;
using GraduationProject.Infrastructure.Repositories;
using GraduationProject.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure
{
    public  static class InfrastructureDependency
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,WebApplicationBuilder builder)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConn"));
            });
            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Generic Repo
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           services.AddInfrastructureRepositories();
            services.AddScoped<IFileServices, FileStorageService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.Configure<JwtSetting>(builder.Configuration.GetSection("JwtSetting"));
            return services;
        }
    }
}


/*
  services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IMedicalRecordRepository, MedicalReportRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IReceptionstRepository, ReceptionistRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IVerificationRepository, VerificationRepository>();
            services.AddScoped<IStudentDoctorRepository, StudentDoctorRepository>();
            services.AddScoped<IAI_ReportRepository, AI_ReportRepository>();
 */