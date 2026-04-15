using FluentValidation;
using GraduationProject.Api.Extensions;
using GraduationProject.Api.Middleware;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Extensions;
using GraduationProject.Application.Features.Patients.Commands.CreatePatient;
using GraduationProject.Data.Identity;
using GraduationProject.Infrastructure;
using GraduationProject.Infrastructure.Context;
using GraduationProject.Infrastructure.ExternalServices;
using GraduationProject.Infrastructure.Persistence.SeedData;
using GraduationProject.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
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

            #region JWT Config
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtSettings:secret"])),
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["jwtSettings:issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["jwtSettings:audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                o.Events = new JwtBearerEvents
                {

                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var response = new
                        {
                            error = "Unauthorized",
                            value = (object?)null
                        };
                        var json = JsonSerializer.Serialize(response);
                        return context.Response.WriteAsync(json);
                    },

                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var response = new
                        {
                            error = "Forbidden",
                            value = (object?)null
                        };
                        var json = JsonSerializer.Serialize(response);
                        return context.Response.WriteAsync(json);
                    }
                };
            });

            #endregion


            builder.Services.AddInfrastructure(builder)
                .AddApplicationServices();

            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;


            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });



            builder.Services.AddApiServices(builder.Configuration);







            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                try
                {
                    await DatabaseSeeder.SeedAsync(serviceProvider);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding roles: {ex.Message}");
                }
            }


            //var services = Assembly.GetExecutingAssembly()
            //    .GetTypes()
            //    .Where(x => x.IsClass && x.Name.EndsWith("Middleware"));
            //foreach(var service in services)
            //{
            //    //builder.Services.AddTransient(typeof(IValidator), service);
            //    Console.WriteLine(service.Name);
            //}

            var services = typeof(CreatePatientValidator).Assembly
                .GetTypes()
                .Where(x => x.IsClass && x.Name.EndsWith("Validator"));
            foreach (var service in services)
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
