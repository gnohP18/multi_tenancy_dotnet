using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tenant_service.Core.Interfaces.Landlord;
using tenant_service.Infrastructure.Database;
using tenant_service.Shared.DTOs.Landlord.Requests.Tenant;

namespace tenant_service.API.Controllers
{
    [ApiController]
    [Tags("Tenant")]
    [Route("tenants")]
    [ApiExplorerSettings(GroupName = "landlord")]
    public class AuthController : ControllerBase
    {
        private readonly LandlordContext _landlordContext;
        private readonly ITenantService _tenantService;

        public AuthController(
            LandlordContext landlordContext,
            ITenantService tenantService)
        {
            _landlordContext = landlordContext ?? throw new ArgumentNullException(nameof(landlordContext));
            _tenantService = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
        }

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetListTenant()
        {
            var result = await _landlordContext.Tenants.Where(_ => _.IsDeleted == false).ToArrayAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantRequest request)
        {
            return await _tenantService.CreateTenantService(request);
        }

        [HttpGet("migrate/{id}")]
        public async Task<IActionResult> MigrateTenant(int id)
        {
            return await _tenantService.MigrateTenantService(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant([FromRoute] int id)
        {
            return await _tenantService.DeleteTenantService(id);
        }

        [HttpGet("sync-redis")]
        public async Task<IActionResult> SyncRedisDomain()
        {
            return await _tenantService.SyncDomain();
        }
    }
}