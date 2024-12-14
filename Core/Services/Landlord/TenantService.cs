using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using DomainModels = tenant_service.Core.Entities.Landlord;
using tenant_service.Shared;
using tenant_service.Infrastructure.Database;
using tenant_service.Application.Exceptions;
using tenant_service.Common;
using tenant_service.Shared.DTOs.Landlord.Requests.Tenant;
using IRedis = StackExchange.Redis.IDatabase;
using tenant_service.Core.Entities;

namespace tenant_service.Core.Interfaces.Landlord
{
    public class TenantService : ITenantService
    {
        private readonly LandlordContext _landlordContext;
        private readonly IConfiguration _configuration;
        private readonly IRedis _redis;

        public TenantService(
            LandlordContext landlordContext,
            IConfiguration configuration,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _landlordContext = landlordContext;
            _configuration = configuration;
            _redis = connectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// Check exist domain by using mysql
        /// Test speed
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true if exist, false if not</returns>
        public async Task<bool> CheckDomainByMysql(TenantDomainRequest request)
        {
            return await _landlordContext.Tenants.AnyAsync(_ => _.Domain == request.Domain);
        }

        /// <summary>
        /// Check exist domain by using redis
        /// Test speed
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true if exist, false if not</returns>
        public async Task<bool> CheckDomainByRedis(TenantDomainRequest request)
        {
            return await _redis.StringGetBitAsync(ConfigGlobal.TenantDomainRedisKey, HashDomain(request.Domain));
        }

        /// <summary>
        /// Create tenant service
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateTenantService(CreateTenantRequest request)
        {
            var tenantId = Guid.NewGuid().ToString();

            var key = HashDomain(request.Domain);
            // Re-check tenantId and re-generate id for tenant if it already exist
            // Maybe we use redis to check it 😑
            while (await _landlordContext.Tenants.AnyAsync(_ => _.TenantId == tenantId))
            {
                tenantId = Guid.NewGuid().ToString();
            }

            // check if domain already exist in redis
            if (await _redis.StringGetBitAsync(ConfigGlobal.TenantDomainRedisKey, key))
            {
                return new BadRequestObjectResult(new { message = $"Tenant with domain {request.Domain} already exists." });
            }

            Console.WriteLine($"Creating Tenant With ID {tenantId}");
            var tenant = new DomainModels.Tenant()
            {
                TenantId = tenantId,
                ApiTenantId = Function.GenerateRandomString((int)TenantEnum.API_TENANT_KEY),
                Domain = request.Domain,
                Note = request.Note,
            };

            // Create tenant database
            await CreateTenantDatabase(tenant);

            // Create tenant in main database
            await _landlordContext.Tenants.AddAsync(tenant);
            await _landlordContext.SaveChangesAsync();

            // Update Redis after create in Mysql
            await _redis.StringSetBitAsync(ConfigGlobal.TenantDomainRedisKey, key, true);

            return new OkObjectResult(new { tenantApiKey = tenant.ApiTenantId });
        }

        /// <summary>
        /// Delete tenant with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<IActionResult> DeleteTenantService(int id)
        {
            var tenant = await GetTenantById(id);

            // Delete schema
            await DeleteTenantDatabase(tenant);

            // Delete domain key bit in redis

            var key = HashDomain(tenant.Domain);

            Console.WriteLine($"Redis bit at {key}");
            await _redis.StringSetBitAsync(ConfigGlobal.TenantDomainRedisKey, key, false);

            _landlordContext.Tenants.Remove(tenant);

            await _landlordContext.SaveChangesAsync();

            return new OkObjectResult(new { message = "Delete tenant successfully" });
        }

        /// <summary>
        /// Migrate tenant base on tenant id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> MigrateTenantService(int id)
        {
            var tenant = await GetTenantById(id);

            var databaseInfo = GetDatabaseInfo();

            var tempConnectionString = Function.GenerateConnectionStringBaseOnTenant(databaseInfo, tenant);

            var dbContextOptions = new DbContextOptionsBuilder<CreatingContext>()
                .UseMySql(tempConnectionString, ConfigGlobal._mySqlServerVersion)
                .Options;
            using var creatingContext = new CreatingContext(dbContextOptions);
            try
            {
                var databaseCreator = creatingContext.Database.GetService<IRelationalDatabaseCreator>();

                if (await creatingContext.Database.CanConnectAsync())
                {
                    await creatingContext.Database.MigrateAsync();
                    Console.WriteLine($"Successfully migrated database for tenant: {tenant.TenantId}");
                }
                else
                {
                    // If we don't find any database -> recreate it!!!!
                    await databaseCreator.CreateAsync();
                    await creatingContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting database: {ex.Message}");
                throw;
            }

            return new OkObjectResult(new { message = "Migrate Tenant Successfully" });
        }

        /// <summary>
        /// Sync domain from mysql to redis using hash crc32 and convert it to uint
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SyncDomain()
        {
            // Clear all key
            await _redis.KeyDeleteAsync(ConfigGlobal.TenantDomainRedisKey);

            // Get all domain
            var domains = await _landlordContext.Tenants.Select(_ => _.Domain).ToListAsync();

            if (domains == null || domains.Count == 0)
            {
                return new BadRequestObjectResult(new { message = "No domains found to sync." });
            }

            var domainDictions = new Dictionary<string, bool>([]);

            foreach (var domain in domains)
            {
                // Set bit for all domain
                await _redis.StringSetBitAsync(ConfigGlobal.TenantDomainRedisKey, HashDomain(domain), true);
                domainDictions[domain] = true;
            }

            return new OkObjectResult(domainDictions);
        }

        /// <summary>
        /// Create tenant database with tenant info
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        protected async Task CreateTenantDatabase(DomainModels.Tenant tenant)
        {
            // Get config and database connection string
            var databaseInfo = GetDatabaseInfo();
            var connectionString = Function.GetConnectionStringServerOnly(databaseInfo);

            // -----------------------Set tem connection string------------------//
            var tempConnectionString = Function.GenerateConnectionStringBaseOnTenant(databaseInfo, tenant);

            var dbContextOptions = new DbContextOptionsBuilder<CreatingContext>()
                .UseMySql(tempConnectionString, ConfigGlobal._mySqlServerVersion)
                .Options;

            using var creatingContext = new CreatingContext(dbContextOptions);

            var databaseCreator = creatingContext.Database.GetService<IRelationalDatabaseCreator>();

            try
            {
                Console.WriteLine($"Database {tenant.TenantId} is being created.");
                await databaseCreator.CreateAsync();
                Console.WriteLine($"Database {tenant.TenantId} is being migrated");
                await creatingContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw;
            }

            // Create and migrate database
            await creatingContext.Database.EnsureCreatedAsync();
            Console.WriteLine($"Database {tenant.TenantId} is now ready.");
        }

        /// <summary>
        /// Delete database tenant
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        protected async Task DeleteTenantDatabase(DomainModels.Tenant tenant)
        {
            var databaseInfo = GetDatabaseInfo();
            var tempConnectionString = Function.GenerateConnectionStringBaseOnTenant(databaseInfo, tenant);
            var dbContextOptions = new DbContextOptionsBuilder<CreatingContext>()
                .UseMySql(tempConnectionString, ConfigGlobal._mySqlServerVersion)
                .Options;
            using var creatingContext = new CreatingContext(dbContextOptions);
            try
            {
                var databaseCreator = creatingContext.Database.GetService<IRelationalDatabaseCreator>();

                if (await creatingContext.Database.CanConnectAsync())
                {
                    await databaseCreator.EnsureDeletedAsync();
                    Console.WriteLine($"Successfully deleted database for tenant: {tenant.TenantId}");
                }
                else
                {
                    Console.WriteLine($"Database for tenant: {tenant.TenantId} does not exist or cannot be connected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting database: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get database info follow app setting
        /// </summary>
        /// <returns></returns>
        private DatabaseInfo GetDatabaseInfo()
        {
            var databaseInfo = new DatabaseInfo();
            _configuration.GetSection(DatabaseInfo.MysqlInfo).Bind(databaseInfo);

            return databaseInfo;
        }

        /// <summary>
        /// Get tenant by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        private async Task<DomainModels.Tenant> GetTenantById(int id)
        {
            var tenant = await _landlordContext.Tenants.FirstOrDefaultAsync(_ => _.Id == id);

            if (tenant is null)
            {
                throw new NotFoundException($"Not found tenant with Id: {id}");
            }

            return tenant;
        }

        /// <summary>
        /// Use salt and HashStringCRC32 to create hash for domain
        /// </summary>
        /// <param name="domain"></param>
        /// <returns>Hashed domain</returns>
        private uint HashDomain(string domain)
        {
            // Get salt
            // Decrease the percentage duplication of domain
            var salt = _configuration.GetSection("TenantSettings")["Key"];

            return Function.HashStringCRC32($"{domain}{salt}");
        }
    }
}