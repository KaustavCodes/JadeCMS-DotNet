using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using JadedCmsCore.Interfaces.Core;

namespace JadedCmsCore.Services.Core;
public class PluginManager
{
    private readonly List<IPlugin> _plugins = new List<IPlugin>();

    public void LoadPlugins(string path, IServiceCollection services, IContentHookManager contentHookManager)
    {
        var pluginFiles = Directory.GetFiles(path, "*.dll");
        foreach (var file in pluginFiles)
        {
            var assembly = Assembly.LoadFrom(file);
            var types = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);
            foreach (var type in types)
            {
                var plugin = (IPlugin)Activator.CreateInstance(type);

                //Initialize the plugin
                plugin.Initialize(services);

                //Register the hooks
                plugin.RegisterContentHooks(contentHookManager);

                //Add the plugin to the list
                _plugins.Add(plugin);
            }
        }
    }

    public void ConfigurePlugins(IApplicationBuilder app, IWebHostEnvironment env)
    {
        foreach (var plugin in _plugins)
        {
            plugin.Configure(app, env);
        }
    }

    public void RegisterRoutes(IEndpointRouteBuilder endpoints)
    {
        foreach (var plugin in _plugins)
        {
            plugin.RegisterRoutes(endpoints);
        }
    }

    public void RegisterViewLocations(IMvcBuilder mvcBuilder)
    {
        foreach (var plugin in _plugins)
        {
            plugin.RegisterViewLocations(mvcBuilder);
        }
    }

    public void ShutdownPlugins()
    {
        foreach (var plugin in _plugins)
        {
            plugin.Shutdown();
        }
    }

    public void EnablePlugin(string name)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Name == name);
        plugin?.Initialize(null);  // Passing null for simplicity
    }

    public void DisablePlugin(string name)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Name == name);
        plugin?.Shutdown();
    }
}