var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IMvcBuilder mvcBuilder = builder.Services.AddControllersWithViews();

var contentHookManager = new JadedCmsCore.Interfaces.Core.ContentHookManager();
builder.Services.AddSingleton<JadedCmsCore.Interfaces.Core.IContentHookManager>(contentHookManager);

// Initialize PluginManager and load plugins
var pluginManager = new JadedCmsCore.Services.Core.PluginManager();
pluginManager.LoadPlugins(Path.Combine(Directory.GetCurrentDirectory(), "Plugins"), builder.Services, contentHookManager);
pluginManager.RegisterViewLocations(mvcBuilder);

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


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
