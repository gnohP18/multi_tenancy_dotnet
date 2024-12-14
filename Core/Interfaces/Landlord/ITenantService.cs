using Microsoft.AspNetCore.Mvc;
using tenant_service.Shared.DTOs.Landlord.Requests.Tenant;

namespace tenant_service.Core.Interfaces.Landlord
{
    public interface ITenantService
    {
        /// <summary>
        /// Create tenant service
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> CreateTenantService(CreateTenantRequest request);

        /// <summary>
        /// Delete tenant with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        Task<IActionResult> DeleteTenantService(int id);

        /// <summary>
        /// Migrate tenant base on tenant id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IActionResult> MigrateTenantService(int id);

        /// <summary>
        /// Sync domain from mysql to redis using hash crc32 and convert it to uint
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> SyncDomain();

        /// <summary>
        /// Check exist domain by using mysql
        /// Test speed
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true if exist, false if not</returns>
        Task<bool> CheckDomainByMysql(TenantDomainRequest request);

        /// <summary>
        /// Check exist domain by using redis
        /// Test speed
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true if exist, false if not</returns>
        Task<bool> CheckDomainByRedis(TenantDomainRequest request);
    }
}