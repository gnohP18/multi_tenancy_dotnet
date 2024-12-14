using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenant_service.Api.Middlewares
{
    public static class RegisterMiddlewareContainer
    {
        public static IApplicationBuilder UseRegisterMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckDomainMiddleware>();
        }
    }
}