using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using tenant_service.API.Decorators;
using tenant_service.API.IOC;
using tenant_service.Api.Middlewares;
using tenant_service.Infrastructure.Database;
using tenant_service.Shared;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Mysql");

var services = builder.Services;
//--------------------------- Add services to the container. ---------------------------//
DependencyContainer.RegisterServices(services);

//--------------------------- Controller -----------------------------------------------//
services.AddControllers();
services.AddHttpContextAccessor();

//--------------------------- Swagger API ---------------------------------------------//
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(ConfigGlobal.SwaggerLandlordName, new OpenApiInfo
    {
        Version = ConfigGlobal.SwaggerLandlordName,
        Title = "Landlord API V1",
        Description = "An ASP.NET Core Web API for managing Tenant items",
    });

    options.SwaggerDoc(ConfigGlobal.SwaggerTenantName, new OpenApiInfo
    {
        Version = ConfigGlobal.SwaggerTenantName,
        Title = "Tenant API V1",
        Description = "An ASP.NET Core Web API for managing Tenant items",
    });

    options.OperationFilter<TenantHeaderFilter>();
});

//--------------------------- Database - Mysql ------------------------------------//
services.AddDbContext<LandlordContext>(
    dbContextOptions => dbContextOptions
        .UseSnakeCaseNamingConvention()
        .UseMySql(connectionString, ConfigGlobal._mySqlServerVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

var app = builder.Build();
app.UseRouting();
app.UseRegisterMiddleware();
app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        var host = httpReq.Host.Value;

        if (host.Contains(ConfigGlobal.SwaggerTenantName))
        {
            swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
            };

            Console.WriteLine($"Host swagger open in {$"{httpReq.Scheme}://{httpReq.Host.Value}/swagger/index.html?urls.primaryName=Tenant+API"}");
        }
        else
        {
            swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
            };

            Console.WriteLine($"Host swagger open in {$"{httpReq.Scheme}://{httpReq.Host.Value}/swagger/index.html?urls.primaryName=Landlord+API"}");
        }
    });
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/tenant/swagger.json", "Tenant API V1");
    options.SwaggerEndpoint("/swagger/landlord/swagger.json", "Landlord API V1");

    options.RoutePrefix = "swagger";
});
app.MapControllers();

app.Run();

