using Microsoft.EntityFrameworkCore;
using tenant_service.Common;
using tenant_service.Core.Entities;
using tenant_service.Core.Entities.Tenant;
using tenant_service.Infrastructure.Providers;
using tenant_service.Shared;

namespace tenant_service.Infrastructure.Database
{
    public class TenantContext : DbContext
    {
        private readonly TenantProvider _tenantProvider;
        private readonly IConfiguration _configuration;

        public TenantContext(
            DbContextOptions<TenantContext> options,
            IConfiguration configuration,
            TenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; } = null!;

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     TenantModelCreating.OnModelCreating(modelBuilder);
        // }

        public override int SaveChanges()
        {
            AddTimestamps();
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimestamps();
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {
                var now = DateTime.Now;
                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).CreatedAt = now;
                    continue;
                }
                ((BaseModel)entity.Entity).UpdatedAt = now;
            }
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseModel)
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDeleted"] = true;
                            break;
                    }
            }
        }

        /// <summary>
        /// Override configuration when get tenant by provider
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databaseInfo = new DatabaseInfo();
            _configuration.GetSection(DatabaseInfo.MysqlInfo).Bind(databaseInfo);

            var connectionStr = Function.GenerateConnectionStringBaseOnTenant(databaseInfo, _tenantProvider.GetTenant());

            optionsBuilder.UseMySql(
                connectionStr, ConfigGlobal._mySqlServerVersion)
                .UseSnakeCaseNamingConvention()
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}