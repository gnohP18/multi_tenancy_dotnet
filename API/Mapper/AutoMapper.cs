using AutoMapper;
using tenant_service.Core.Entities.Tenant;
using tenant_service.Shared.DTOs.Tenant.User;

namespace tenant_service.API.Mapper
{
    public class AutoMapper
    {
        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                CreateMap<User, UserListDTO>();
            }
        }

    }
}