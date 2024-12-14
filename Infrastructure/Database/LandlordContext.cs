using Microsoft.EntityFrameworkCore;
using tenant_service.Core.Entities;
using tenant_service.Core.Entities.Landlord;
using tenant_service.Infrastructure.Seeders.Landlord;

namespace tenant_service.Infrastructure.Database
{
    public class LandlordContext : DbContext
    {
        public LandlordContext(DbContextOptions<LandlordContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreating.OnModelCreating(modelBuilder);
            LandlordDatabaseSeeder.SeedData(modelBuilder);
        }

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
    }
}