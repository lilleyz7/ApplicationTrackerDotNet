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
using ApplicationTracker.Redis;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>     
{
    options.AddPolicy(name: "DevelopmentPolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); ;
                      });
});

builder.Services.AddAuthorization();

builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

// Add services to the container.
var env = builder.Environment;

if (env.IsDevelopment())
{
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlite($"Data Source=app.db"));

    //builder.Services.AddStackExchangeRedisCache(options =>
    //{
    //    options.Configuration = builder.Configuration.GetConnectionString("dev_redis");
    //    Console.WriteLine(builder.Configuration.GetConnectionString("dev_redis"));
    //    options.InstanceName = "DevApplications";
    //});

}
else
{
    var prodConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(prodConnectionString));

    //builder.Services.AddStackExchangeRedisCache(options =>
    //{
    //    options.Configuration = builder.Configuration.GetConnectionString("prod_redis");
    //    options.InstanceName = "Applications";
    //});
}

builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IApplicationRepo, ApplicationRepo>();
//builder.Services.AddScoped<IRedisService, RedisCacheService>();

var app = builder.Build();

app.MapIdentityApi<CustomUser>();
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
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
