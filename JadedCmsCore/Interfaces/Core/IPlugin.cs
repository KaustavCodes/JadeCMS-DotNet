using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace JadedCmsCore;

public interface IPlugin
{
    string Name { get; }
    void Initialize(IServiceCollection services);
    void Configure(IApplicationBuilder app, IWebHostEnvironment env);
    void RegisterRoutes(IEndpointRouteBuilder endpoints);
    void RegisterViewLocations(IMvcBuilder mvcBuilder);
    void Shutdown();
}
