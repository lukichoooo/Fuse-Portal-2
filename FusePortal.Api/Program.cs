using System.Security.Claims;
using System.Text;
using FusePortal.Api.Settings;
using FusePortal.Application;
using FusePortal.Application.Common.Settings;
using FusePortal.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);


// settings
builder.Services.Configure<ControllerSettings>(
        builder.Configuration.GetSection("ControllerSettings"));
builder.Services.Configure<ValidatorSettings>(
        builder.Configuration.GetSection("ValidatorSettings"));

// configure Auth Middleware
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        RoleClaimType = ClaimTypes.Role,
    };
});

var app = builder.Build();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();

app.Run();

