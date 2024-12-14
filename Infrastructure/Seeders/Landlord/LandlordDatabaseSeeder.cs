using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tenant_service.Infrastructure.Seeders.Landlord
{
    public class LandlordDatabaseSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            TenantLandlordSeeder.SeedData(modelBuilder);
        }
    }
}