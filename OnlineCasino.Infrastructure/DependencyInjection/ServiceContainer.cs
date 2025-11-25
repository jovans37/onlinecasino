using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Interfaces;
using OnlineCasino.Infrastructure.Data;
using OnlineCasino.Infrastructure.Repositories;
using OnlineCasino.Infrastructure.Services;
using OnlineCasino.SharedLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnectionString")));

            services.AddScoped<IBonusRepository, BonusRepository>();
            services.AddScoped<IBonusAuditLogRepository, BonusAuditLogRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<JwtSettings>(options =>
            {
                options.Secret = configuration["JwtSettings:Secret"]!;
                options.Issuer = configuration["JwtSettings:Issuer"]!;
                options.Audience = configuration["JwtSettings:Audience"]!;
                options.ExpiryMinutes = int.Parse(configuration["JwtSettings:ExpiryMinutes"]!);
            });

            services.AddScoped<IJwtService, JwtService>();

            return services;
        }

        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            return app;
        }
    }
}
