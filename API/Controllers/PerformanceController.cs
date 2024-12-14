using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tenant_service.Shared.DTOs.Landlord.Requests.Tenant;
using tenant_service.Core.Interfaces.Landlord;

namespace tenant_service.API.Controllers
{
    [ApiController]
    [Tags("Performance")]
    [Route("performance")]
    [ApiExplorerSettings(GroupName = "landlord")]
    public class PerformanceController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public PerformanceController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet("check-domain-mysql")]
        public async Task<ActionResult<bool>> CheckDomainMysql([FromBody] TenantDomainRequest request)
        {
            return Ok(await _tenantService.CheckDomainByMysql(request));
        }

        [HttpGet("check-domain-redis")]
        public async Task<ActionResult<bool>> CheckDomainRedis([FromBody] TenantDomainRequest request)
        {
            return Ok(await _tenantService.CheckDomainByRedis(request));
        }
    }
}