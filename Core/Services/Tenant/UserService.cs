using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tenant_service.Core.Entities.Tenant;
using tenant_service.Infrastructure.Database;
using tenant_service.Shared.DTOs.Tenant.User;

namespace tenant_service.Core.Interfaces.Tenant
{
    public class UserService : IUserService
    {
        private readonly TenantContext _tenantContext;
        private readonly IMapper _mapper;

        public UserService(
            TenantContext tenantContext,
            IMapper mapper)
        {
            _tenantContext = tenantContext ?? throw new ArgumentNullException(nameof(tenantContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
        }

        public async Task<List<UserListDTO>> GetUserListService()
        {
            var result = await _tenantContext.Users
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();

            return result.Select(_ => _mapper.Map<User, UserListDTO>(_)).ToList();
        }
    }
}