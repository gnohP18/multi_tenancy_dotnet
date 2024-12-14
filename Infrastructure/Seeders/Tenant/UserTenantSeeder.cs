using Microsoft.EntityFrameworkCore;
using tenant_service.Core.Entities.Tenant;


namespace tenant_service.Infrastructure.Seeders.Tenant
{
    public class UserTenantSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Uzumaki Naruto", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false },
                new User { Id = 2, Name = "Hayate Kakashi", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false },
                new User { Id = 3, Name = "Uchiha Madara", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false }
            );
        }
    }
}