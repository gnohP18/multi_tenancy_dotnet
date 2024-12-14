using Microsoft.EntityFrameworkCore;
using tenant_service.Infrastructure.Seeders.Tenant;

namespace tenant_service.Infrastructure.Database
{
    public class CreatingContext : DbContext
    {
        /// <summary>
        /// This class helps you can create database
        /// Use output from Migrations/tenants
        /// </summary>
        public CreatingContext(DbContextOptions<CreatingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            TenantModelCreating.OnTenantModelCreating(modelBuilder);
            TenantDatabaseSeeder.SeedData(modelBuilder);
        }
    }
}