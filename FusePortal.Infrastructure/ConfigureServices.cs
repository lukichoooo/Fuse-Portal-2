
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Repo;
using FusePortal.Application.Interfaces;
using FusePortal.Infrastructure.Authenticatoin;
using FusePortal.Infrastructure.Settings.Auth;

namespace FusePortal.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("AppDbContext")));

            services.AddScoped<IUserRepo, UserRepo>();

            // auth
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEncryptor, Encryptor>();
            services.AddScoped<IAuthService, AuthService>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EncryptorSettings>(configuration.GetSection("EncryptorSettings"));

            return services;
        }
    }
}
