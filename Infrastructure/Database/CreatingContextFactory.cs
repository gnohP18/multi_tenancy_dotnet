using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using tenant_service.Shared;

namespace tenant_service.Infrastructure.Database
{
    /// <summary>
    /// This class helps you can add migration for tenant
    /// Output migration file will be added into directory Migrations/Tenants
    /// For adding new migration, use command:
    /// ```
    ///     dotnet ef migrations add {migration_name} -o Migrations/Tenants --context CreatingContext
    /// ```
    /// </summary>
    public class CreatingContextFactory : IDesignTimeDbContextFactory<CreatingContext>
    {
        public CreatingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CreatingContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("TenantMysql");

            optionsBuilder.UseMySql(connectionString, ConfigGlobal._mySqlServerVersion)
                .UseSnakeCaseNamingConvention()
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            return new CreatingContext(optionsBuilder.Options);
        }
    }
}