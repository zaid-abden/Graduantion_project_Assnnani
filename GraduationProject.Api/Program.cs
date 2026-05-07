using FluentValidation;

using GraduationProject.Api.Extensions;
using GraduationProject.Application.BackgroundJobs.Appointments;
using GraduationProject.Application.BackgroundJobs.Patients;
using GraduationProject.Application.Contracts.ExternalServices;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Application.Extensions;

//using GraduationProject.Application.Features.Patients.Commands.CreatePatient;
using GraduationProject.Infrastructure;
using GraduationProject.Infrastructure.ExternalServices;
using GraduationProject.Infrastructure.Persistence.SeedData;
using GraduationProject.Infrastructure.Repositories;
using GraduationProject.Infrastructure.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
namespace GraduationProject.Api
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// ✅ 1. أول حاجة: إضافة الـ Services الأساسية
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddHttpContextAccessor();

			// ✅ 2. Hangfire
			builder.Services.AddHangfire(config =>
				config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DBConn")));
			builder.Services.AddHangfireServer();

			// ✅ 3. Services بتاعتك (تأكد إنهم مش بيضيفوا حاجة بعد Build)
			builder.Services.AddInfrastructure(builder)
							.AddApplicationServices();

			builder.Services.AddScoped<IFileServices, FileStorageService>();
			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();

			builder.Services.AddApiServices(builder.Configuration);

			// ✅ 4. FluentValidation settings
			ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

			// ✅ 5. API Behavior
			builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

			// ✅ 6. JSON Options
			builder.Services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});

			// ✅ 7. Swagger
			builder.Services.AddSwaggerGen(options =>
			{
				options.EnableAnnotations();
				options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
					securityScheme: new OpenApiSecurityScheme
					{
						Name = "Authorization",
						Description = "Enter the Bearer Authorization : `Bearer Genreated-JWT-Token`",
						In = ParameterLocation.Header,
						Type = SecuritySchemeType.ApiKey,
						Scheme = "Bearer"
					});

				options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = JwtBearerDefaults.AuthenticationScheme
					}
				},
				new string[] { }
			}
		});
			});

			// ✅ 8. Authentication & JWT
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(o =>
			{
				o.RequireHttpsMetadata = false;
				o.SaveToken = false;
				o.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
					ValidAudience = builder.Configuration["JwtSetting:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:Key"]))
				};
			});

			// ✅ 9. CORS (قبل Build)
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("mypolicy1", policy =>
				{
					policy.AllowAnyOrigin()
						  .AllowAnyMethod()
						  .AllowAnyHeader();
				});
			});

			// ✅ 10. Build (هنا بتتجمد الخدمات)
			var app = builder.Build();

			// ✅ 11. بعد Build: Middleware بس، مش إضافة خدمات جديدة
			app.UseHangfireDashboard("/hangfire");

			// ✅ 12. Seed data
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

			// ✅ 13. Middleware pipeline
			app.UseStaticFiles();
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseCors("mypolicy1");
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			// ✅ 14. Recurring jobs
			RecurringJob.AddOrUpdate<IPatientStatusService>(
				"inactive-patients-job",
				x => x.UpdateInactivePatients(CancellationToken.None),
				Cron.Minutely
			);

			RecurringJob.AddOrUpdate<IAppointmentJobService>(
				"mark-no-show",
				x => x.MarkNoShowAppointments(CancellationToken.None),
				Cron.Minutely
			);

			app.Run();
		}
	}
}
