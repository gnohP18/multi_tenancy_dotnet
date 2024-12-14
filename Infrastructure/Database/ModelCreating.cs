using Microsoft.EntityFrameworkCore;
using tenant_service.Core.Entities.Landlord;

namespace tenant_service.Infrastructure.Database
{
    public static class ModelCreating
    {
        public static ModelBuilder OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property<string>(entity => entity.TenantId)
                    .HasColumnName("tenant_id")
                    .HasColumnType("varchar(255)")
                    .IsRequired(true);

                entity.Property<string>(entity => entity.ApiTenantId)
                    .HasColumnName("api_tenant_id")
                    .HasColumnType("varchar(255)")
                    .IsRequired(true);

                entity.Property<string>(entity => entity.Domain)
                    .HasColumnName("domain")
                    .HasColumnType("varchar(255)")
                    .IsRequired(true);

                entity.Property<string?>(entity => entity.Note)
                    .HasColumnName("note")
                    .HasColumnType("text")
                    .IsRequired(false);

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
            builder.Entity<Tenant>().HasQueryFilter(x => x.IsDeleted == false);
            return builder;
        }
    }
}