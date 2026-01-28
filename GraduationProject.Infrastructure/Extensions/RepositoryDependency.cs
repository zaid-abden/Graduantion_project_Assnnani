using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Extensions
{
    public static class RepositoryDependency
    {
       public static IServiceCollection AddInfrastructureRepositories(
        this IServiceCollection services)
        {
            var infraAssembly = typeof(UnitOfWork).Assembly;

            var implementations = infraAssembly.GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericTypeDefinition);

            foreach (var impl in implementations)
            {
                var interfaces = impl.GetInterfaces()
                    .Where(i =>
                        i.Namespace != null &&
                        i.Namespace.Contains("Application.Contracts.Repositories"));

                foreach (var i in interfaces)
                {
                    services.AddScoped(i, impl);
                }
            }

            return services;
        }
    }
    
}
