using JadedCmsCore.Interfaces.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace DemoPlugin;

public class DemoPlugin: IPlugin
{
    public string Name => "DemoPlugin";
    public string Description => "A sample plugin for demonstration purposes";
    public string Author => "Kaustav Halder";

    public void Initialize(IServiceCollection services)
    {
        // Add services required by the plugin
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Middleware configuration for the plugin
    }

    public void RegisterRoutes(IEndpointRouteBuilder endpoints)
    {
        // Define routes for the plugin
        endpoints.MapControllerRoute(
            name: "DemoPluginRoute",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    }

    public void RegisterViewLocations(IMvcBuilder mvcBuilder)
    {
        // Register views and view components
        var assembly = typeof(DemoPlugin).Assembly;
        mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
    }

    public void Shutdown()
    {
        // Cleanup resources if needed
    }
}
