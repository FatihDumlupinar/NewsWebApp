using Microsoft.IdentityModel.Logging;
using NewsWeb.Application.Extensions;
using NewsWeb.Application.Seed;
using NewsWeb.Persistence.Extensions;
using NewsWeb.WebApp.Utilities.Extensions;
using NewsWeb.WebApp.Utilities.Handlers;
using NewsWeb.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = AppsettingsConfig.AddAppsettingsConfig();

builder.UseCustomSerilogConfig();

builder.Services.AddHttpLoggingConfig();

builder.Services.AddDbContextConfig(config);

builder.Services.AddApplicationDependecyConfig();

builder.Services.AddInfrastructureDependecyConfig();

builder.Services.AddWebApiDependecyConfig();

builder.Services.AddRedisConfig(config);

builder.Services.AddControllersWithViews();

builder.Services.AddIdentityConfig();

builder.Services.AddInstanceConfig(config);

if (builder.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;//development modda identity içindeki hatalarý göstermek için
}

builder.Services.AddCorsConfig();

var app = builder.Build();

app.UseCustomExceptionHandler(builder.Environment);

app.UseCustomCors();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityAuth();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.Services.EnsurePopulatedAsync();

app.Run();


