using System.Reflection;
using IdentityMembersipAuthServer.Data;
using IdentityMembersipAuthServer.Models;
using IdentityMembersipAuthServer.Services;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLocalApiAuthentication();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

var assemblyName = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
    options.EmitStaticAudienceClaim = true;
})
.AddConfigurationStore(options =>
{
    options.ConfigureDbContext = context =>
    {
        context.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), action =>
        {
            action.MigrationsAssembly(assemblyName);
        });
    };
})
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = context =>
    {
        context.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), action =>
        {
            action.MigrationsAssembly(assemblyName);
        });
    };
})
.AddAspNetIdentity<AppUser>()
.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
.AddDeveloperSigningCredential();

builder.Services.AddAuthentication()
.AddGoogle(options =>
{
    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
    options.ClientId = "copy client ID from Google here";
    options.ClientSecret = "copy client secret from Google here";
});

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
            .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

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

app.UseIdentityServer();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

await Seeds.Seed(app.Services.GetRequiredService<ConfigurationDbContext>());

app.Run();
