using Microsoft.AspNetCore.Mvc;
using tenant_service.Core.Interfaces.Tenant;

namespace tenant_service.API.Controllers.Tenant
{
    [ApiController]
    [Tags("User")]
    [Route("user")]
    [ApiExplorerSettings(GroupName = "tenant")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("test")]
        public async Task<ActionResult<List<object>>> GetListUser()
        {
            return Ok(await _userService.GetUserListDTO());
        }
    }
}