using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace tenant_service.Shared
{
    public static class ConfigGlobal
    {
        /// <summary>
        /// Landlord name for swagger
        /// </summary>
        public static string SwaggerLandlordName = "landlord";

        /// <summary>
        /// Tenant name for swagger
        /// </summary>
        public static string SwaggerTenantName = "tenant";

        /// <summary>
        /// Version for mysql database
        /// </summary>
        public static Version _version = new Version(8, 4, 0);

        /// <summary>
        /// Represents a ServerVersion for MySQL database servers
        /// </summary>
        public static MySqlServerVersion _mySqlServerVersion = new MySqlServerVersion(_version);

        /// <summary>
        /// Header key for tenant
        /// </summary>
        public static string TenantHeader = "X-Tenant-Api-key";

        /// <summary>
        /// Header key for landlord
        /// </summary>
        public static string LandlordHeader = "Landlord-Secret-key";

        /// <summary>
        /// Number of user allow Redis
        /// </summary>
        public static uint MaxAllowUser = 1_000_000;

        /// <summary>
        /// Redis key for tenant
        /// </summary>
        public static RedisKey TenantDomainRedisKey = "tenant:domain";
    }
}