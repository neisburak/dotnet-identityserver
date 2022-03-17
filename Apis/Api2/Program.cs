using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://localhost:7000";
    options.Audience = "resource_api2";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Read", policy =>
    {
        policy.RequireClaim("scope", "api2.read");
    });

    options.AddPolicy("Upsert", policy =>
    {
        policy.RequireClaim("scope", "api2.upsert");
    });

    options.AddPolicy("Delete", policy =>
    {
        policy.RequireClaim("scope", "api2.delete");
    });
});

builder.Services.AddCors(p => p.AddPolicy("AngularAppPolicy", builder =>
{
    builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularAppPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
