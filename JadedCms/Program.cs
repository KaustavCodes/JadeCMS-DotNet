using JadedCmsCore.Interfaces.Database;
using JadedCmsCore.Services.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IMvcBuilder mvcBuilder = builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var contentHookManager = new JadedCmsCore.Interfaces.Core.ContentHookManager();
builder.Services.AddSingleton<JadedCmsCore.Interfaces.Core.IContentHookManager>(contentHookManager);

// Initialize PluginManager and load plugins
var pluginManager = new JadedCmsCore.Services.Core.PluginManager();
pluginManager.LoadPlugins(Path.Combine(Directory.GetCurrentDirectory(), "Plugins"), builder.Services);
pluginManager.RegisterViewLocations(mvcBuilder);

//Setup the databse
builder.Services.AddSingleton<DatabaseConfigurationService>();

// Use the configuration service to determine which database service to register
var dbServiceProvider = builder.Services.BuildServiceProvider();
var databaseConfigService = dbServiceProvider.GetService<DatabaseConfigurationService>();
var databaseType = databaseConfigService.GetDatabaseType();

switch (databaseType)   
{
    case "MsSql":
        builder.Services.AddSingleton<IDatabaseService, MsSqlDbService>();
        break;
    case "MySql":
        builder.Services.AddSingleton<IDatabaseService, MsSqlDbService>();
        break;
    // Add cases for other database types
    default:
        throw new Exception("Unsupported database type");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Custom Plugin Injection Middleware
pluginManager.ConfigurePlugins(app, app.Environment);

app.UseEndpoints(endpoints =>
{
    // Custom Plugin Injection for Endpoints
    pluginManager.RegisterRoutes(endpoints);
});

IServiceProvider serviceProvider = app.Services;

pluginManager.ConfigureHooks(contentHookManager, serviceProvider);


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
