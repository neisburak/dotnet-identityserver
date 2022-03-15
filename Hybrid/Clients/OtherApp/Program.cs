using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Cookies
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; // OpenIdConnect
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = "https://localhost:7000";
    options.ClientId = builder.Configuration.GetValue<string>("Client:Id");
    options.ClientSecret = builder.Configuration.GetValue<string>("Client:Secret");
    options.ResponseType = "code id_token";
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;

    options.Scope.Add("api1.read");
    options.Scope.Add("offline_access");
    options.Scope.Add("CountryAndCity");
    options.Scope.Add("Roles");
    options.ClaimActions.MapUniqueJsonKey("country", "country");
    options.ClaimActions.MapUniqueJsonKey("city", "city");
    options.ClaimActions.MapUniqueJsonKey("role", "role");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role",
    };
});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
