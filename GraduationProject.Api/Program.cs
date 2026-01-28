
using Azure.Identity;
using FluentValidation;
using GraduationProject.Api.Middleware;
using GraduationProject.Application.Extensions;
using GraduationProject.Application.Features.Patients.Commands.CreatePatient;
using GraduationProject.Infrastructure;
using GraduationProject.Infrastructure.Persistence.SeedData;
using System.Reflection;
namespace GraduationProject.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



         builder.Services.AddInfrastructure(builder)
                .AddApplicationServices();

           ValidatorOptions.Global.DefaultClassLevelCascadeMode=CascadeMode.Stop;

            var app = builder.Build();


            //var services = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .Where(x => x.IsClass && x.Name.EndsWith("Middleware"));
            //foreach(var service in services)
            //{
            //    //builder.Services.AddTransient(typeof(IValidator), service);
            //    Console.WriteLine(service.Name);
            //}
           
            var services=typeof(CreatePatientValidator).Assembly
                .GetTypes()
                .Where(x=>x.IsClass&&x.Name.EndsWith("Validator"));
            foreach(var service in services)
            {
                //builder.Services.AddTransient(typeof(IValidator), service);
                Console.WriteLine(service.Name);
            }

            // RUN SEEDERS
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    await DatabaseSeeder.SeedAsync(services);
            //}




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
