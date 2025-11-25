using Microsoft.Extensions.DependencyInjection;
using OnlineCasino.Application.Interfaces;
using OnlineCasino.Application.Mapping;
using OnlineCasino.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCasino.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;

            }, Assembly.GetExecutingAssembly());

            services.AddScoped<IBonusService, BonusService>();
            services.AddScoped<IBonusAuditLogService, BonusAuditLogService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }   
    }
}
