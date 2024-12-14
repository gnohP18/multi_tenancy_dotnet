using Microsoft.EntityFrameworkCore;
using tenant_service.Core.Entities.Tenant;

namespace tenant_service.Infrastructure.Database
{
    public static class TenantModelCreating
    {
        public static ModelBuilder OnTenantModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property<string>(entity => entity.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .IsRequired(true);

                entity.Property<DateTime>(entity => entity.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime")
                    .IsRequired(true);

                entity.Property<DateTime>(entity => entity.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime")
                    .IsRequired(true);

                entity.Property<bool>(entity => entity.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("TINYINT(1)")
                    .IsRequired(true)
                    .HasDefaultValue(false);
            });
            builder.Entity<User>().HasQueryFilter(x => x.IsDeleted == false);

            return builder;
        }
    }
}