
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Repo;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using FusePortal.Application.Interfaces.Auth;

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
            services.AddHttpContextAccessor();
            services.AddScoped<IUserSecurityService, UserSecurityService>();
            services.AddScoped<ICurrentContext, CurrentContext>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEncryptor, Encryptor>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EncryptorSettings>(configuration.GetSection("EncryptorSettings"));

            return services;
        }
    }
}
