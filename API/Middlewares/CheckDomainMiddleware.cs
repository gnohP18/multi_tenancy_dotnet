using StackExchange.Redis;
using tenant_service.Application.Exceptions;
using tenant_service.Shared;

namespace tenant_service.Api.Middlewares
{
    public class CheckDomainMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IDatabase _redis;

        public CheckDomainMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            IConnectionMultiplexer connectionMultiplexer)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _redis = connectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// Explain the function
        /// 1. We hash Domain with CRC32 and get number of it
        /// Example abc-def -> 123888 -> SET bit 123888 1 . It means at memory index 123888 we have value 1
        /// 2. We use number to get bit of memory 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task InvokeAsync(HttpContext context)
        {
            // Check Landlord first
            var landlordSecretKey = _configuration.GetSection("LandlordSettings")["LandlordSecretKey"];

            if (context.Request.Headers.TryGetValue(ConfigGlobal.LandlordHeader, out var landlordHeaderSecretKey)
                && landlordSecretKey == landlordHeaderSecretKey)
            {
                await _next(context); // Skip middleware
                return;
            }

            // Check available tenant second
            // Check Domain
            var isCheckDomain = _configuration.GetSection("TenantSettings")["AppCheckDomain"] == "True";
            var domain = "";
            if (isCheckDomain)
            {
                // Get domain
                domain = context.Request.Host.Value.Split('.')[0];

                var salt = _configuration.GetSection("TenantSettings")["Key"];
                // HashDomain
                var hashDomain = Function.HashStringCRC32($"{domain}{salt}");

                if (!await _redis.StringGetBitAsync(ConfigGlobal.TenantDomainRedisKey, hashDomain))
                {
                    throw new NotFoundException($"Unknown Domain {domain}");
                };

                if (!context.Request.Headers.TryGetValue(ConfigGlobal.TenantHeader, out var tenantApiKey) || string.IsNullOrWhiteSpace(tenantApiKey))
                {
                    throw new BadRequestException($"Missing {ConfigGlobal.TenantHeader} in Header");
                }
            }

            await _next(context);
        }
    }
}