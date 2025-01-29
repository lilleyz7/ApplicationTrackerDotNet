using ApplicationTracker.Data;
using ApplicationTracker.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
var env = builder.Environment;

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseSqlite($"Data Source=app.db");  
    }
    else
    {
        // production database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString);
    }
});
builder.Services.AddIdentityApiEndpoints<CustomUser>()
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

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
