
using FluentValidation;
using GraduationProject.Api.Middleware;
using GraduationProject.Application.Extensions;
using GraduationProject.Infrastructure;
using GraduationProject.Infrastructure.Persistence.SeedData;
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

           

            var app = builder.Build();

            // RUN SEEDERS
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DatabaseSeeder.SeedAsync(services);
            }




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
