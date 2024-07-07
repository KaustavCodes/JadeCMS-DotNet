using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;

namespace JadedCmsCore.Interfaces.Core;

public interface IPlugin
{
    string Name { get; }

    string Description { get; }

    string Author { get; }
    
    void Initialize(IServiceCollection services);
    void Configure(IApplicationBuilder app, IWebHostEnvironment env);
    void RegisterRoutes(IEndpointRouteBuilder endpoints);
    void RegisterViewLocations(IMvcBuilder mvcBuilder);

    void RegisterContentHooks(IContentHookManager hookManager);
    void Shutdown();
}
