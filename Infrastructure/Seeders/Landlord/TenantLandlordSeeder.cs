using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tenant_service.Shared;
using Entity = tenant_service.Core.Entities.Landlord;

namespace tenant_service.Infrastructure.Seeders.Landlord
{
    public class TenantLandlordSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity.Tenant>().HasData(
                new Entity.Tenant()
                {
                    Id = 1,
                    TenantId = Guid.NewGuid().ToString(),
                    ApiTenantId = Function.GenerateRandomString(),
                    Domain = "shoes-store",
                    Note = "This is an example tenant"
                },
                new Entity.Tenant()
                {
                    Id = 2,
                    TenantId = Guid.NewGuid().ToString(),
                    ApiTenantId = Function.GenerateRandomString(),
                    Domain = "shuba-grocery",
                    Note = "This is an example tenant"
                },
                new Entity.Tenant()
                {
                    Id = 3,
                    TenantId = Guid.NewGuid().ToString(),
                    ApiTenantId = Function.GenerateRandomString(),
                    Domain = "max-toys-store",
                    Note = "This is an example tenant"
                }
            );
        }
    }
}