using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using tenant_service.Shared;

namespace tenant_service.API.Decorators
{
    public class TenantHeaderFilter : IOperationFilter
    {
        private readonly IConfiguration _configuration;

        public TenantHeaderFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>([]);
            }
            //------------------------------ Landlord ------------------------------//
            if (context.DocumentName == ConfigGlobal.SwaggerLandlordName)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = ConfigGlobal.LandlordHeader,
                    In = ParameterLocation.Header,
                    Required = _configuration.GetSection("TenantSettings")["AppCheckDomain"] == "True"
                });
            }
            //------------------------------ Tenant -------------------------------//
            else if (context.DocumentName == ConfigGlobal.SwaggerTenantName)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = ConfigGlobal.TenantHeader,
                    In = ParameterLocation.Header,
                    Required = true
                });
            }
        }
    }
}