using Microsoft.EntityFrameworkCore;
using tenant_service.Application.Exceptions;
using tenant_service.Core.Entities.Landlord;
using tenant_service.Infrastructure.Database;
using tenant_service.Shared;

namespace tenant_service.Infrastructure.Providers
{
    /// <summary>
    /// This class determined which tenant is connecting
    /// </summary>
    public sealed class TenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LandlordContext _landlordContext;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, LandlordContext landlordContext)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _landlordContext = landlordContext ?? throw new ArgumentNullException(nameof(landlordContext)); ;
        }

        /// <summary>
        /// Get Tenant base on Header
        /// </summary>
        /// <returns>Tenant</returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="NotFoundException"></exception>
        public Tenant GetTenant()
        {
            if (_httpContextAccessor.HttpContext == null
                || !_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(ConfigGlobal.TenantHeader, out var tenantHeader)
                || string.IsNullOrWhiteSpace(tenantHeader))
            {
                throw new BadRequestException($"Missing {ConfigGlobal.TenantHeader} at Header");
            }

            var tenant = _landlordContext.Tenants.FirstOrDefault(_ => _.ApiTenantId == tenantHeader.ToString());

            if (tenant is null)
            {
                throw new NotFoundException("Tenant not found");
            }

            return tenant;
        }

        /// <summary>
        /// Check tenant is exist
        /// </summary>
        /// <param name="tenantApiKey"></param>
        /// <param name="domain"></param>
        /// <param name="isCheckDomain"></param>
        /// <returns>true if tenant exist, false if not</returns>
        public async Task<bool> CheckTenantIsExist(string tenantApiKey, string domain, bool isCheckDomain = false)
        {
            var query = _landlordContext.Tenants.Where(tenant => tenant.ApiTenantId == tenantApiKey);

            if (isCheckDomain)
            {
                query = query.Where(tenant => tenant.Domain.ToLower() == domain.ToLower());
            }

            var tenant = await query.FirstOrDefaultAsync();

            return tenant is not null;
        }
    }
}