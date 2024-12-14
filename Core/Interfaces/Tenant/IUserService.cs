using tenant_service.Shared.DTOs.Tenant.User;

namespace tenant_service.Core.Interfaces.Tenant
{
    public interface IUserService
    {
        /// <summary>
        /// Get list user
        /// </summary>
        /// <returns></returns>
        Task<List<UserListDTO>> GetUserListDTO();
    }
}