using GraduationProject.Application.Settings;

namespace GraduationProject.Api.Extensions
{
    public static class ApiServiceRegisteration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            RegisterServicesAndSettings(services, configuration);
            return services;
        }

        private static void RegisterServicesAndSettings(IServiceCollection services, IConfiguration configuration)
        {
           
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
        }
    }
}
