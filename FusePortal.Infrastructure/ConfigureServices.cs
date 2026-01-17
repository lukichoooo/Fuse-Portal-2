
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FusePortal.Infrastructure.Repo;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Application.Common;
using FusePortal.Domain.Entities.UniversityAggregate;
using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Infrastructure.EventDispatcher;

namespace FusePortal.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            // external
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("AppDbContext")));

            // settings
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EncryptorSettings>(configuration.GetSection("EncryptorSettings"));


            // auth
            services.AddHttpContextAccessor();
            services.AddScoped<IUserSecurityService, UserSecurityService>();
            services.AddScoped<IIdentityProvider, IdentityProvider>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEncryptor, Encryptor>();


            // repo
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUniRepo, UniRepo>();


            // messaging
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


            return services;
        }
    }
}
