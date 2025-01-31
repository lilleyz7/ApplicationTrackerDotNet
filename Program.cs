using ApplicationTracker.Data;
using ApplicationTracker.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApplicationTracker.Repos;
using Microsoft.Extensions.Configuration;
using System;

var FrontEndPolicy = "frontendpolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>     
{
    options.AddPolicy(name: FrontEndPolicy,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); ;
                      });
});

builder.Services.AddAuthorization();

// Add services to the container.
var env = builder.Environment.EnvironmentName;

if (env == "Development")
{
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlite($"Data Source=app.db"));
}
else
{
    var prodConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(prodConnectionString));
}

builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IApplicationRepo, ApplicationRepo>();

var app = builder.Build();

app.UseCors(FrontEndPolicy);

app.MapIdentityApi<CustomUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
