using JadedCmsCore.Interfaces.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

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
            pattern: "DemoPlugin/{controller=Demo}/{action=Index}/{id?}");
    }

    public void RegisterViewLocations(IMvcBuilder mvcBuilder)
    {
        // Register views and view components
        var assembly = typeof(DemoPlugin).Assembly;
        mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
        mvcBuilder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
        {
            options.FileProviders.Add(new EmbeddedFileProvider(assembly));
        });
    }

    public void RegisterContentHooks(IContentHookManager hookManager)
    {
        hookManager.RegisterContent("BeforeContent", () => "This is text from plugin. Should come before content.");

        hookManager.RegisterContent("AfterContent", () => "This is text from plugin. Should come after content.");
    }

    public void Shutdown()
    {
        // Cleanup resources if needed
    }
}
