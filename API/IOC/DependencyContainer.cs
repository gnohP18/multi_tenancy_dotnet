using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using tenant_service.Shared;
using tenant_service.Core.Interfaces.Landlord;
using tenant_service.Infrastructure.Database;
using tenant_service.Infrastructure.Providers;
using static tenant_service.API.Mapper.AutoMapper;
using tenant_service.Core.Interfaces.Tenant;
using tenant_service.Core.Entities;

namespace tenant_service.API.IOC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //------------------------------ Provider ------------------------------//
            services.AddScoped<TenantProvider>();

            //------------------------------ Decorate ------------------------------//


            //------------------------------ Mapper --------------------------------//
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //------------------------------ Database mysql tenant -----------------//
            services.AddDbContext<TenantContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var tenantProvider = serviceProvider.GetRequiredService<TenantProvider>();

                var databaseInfo = new DatabaseInfo();
                configuration.GetSection(DatabaseInfo.MysqlInfo).Bind(databaseInfo);

                // var connectionStr = Common.Function.GetConnectionStringServerOnly(databaseInfo);
                var connectionStr = configuration.GetConnectionString("TenantMysql");
                options.UseMySql(connectionStr, ConfigGlobal._mySqlServerVersion)
                    .UseSnakeCaseNamingConvention()
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            //------------------------------ Database Redis -----------------------//
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

            // Add your mapping service here
            //---------------------------- Landlord ----------------------------//
            // services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<ITenantService, TenantService>();

            //---------------------------- Tenant -----------------------------//
            services.AddScoped<IUserService, UserService>();
        }

    }
}