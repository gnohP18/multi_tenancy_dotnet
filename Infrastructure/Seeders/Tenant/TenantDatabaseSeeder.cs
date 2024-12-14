using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tenant_service.Infrastructure.Seeders.Tenant
{
    public class TenantDatabaseSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            UserTenantSeeder.SeedData(modelBuilder);
        }
    }
}