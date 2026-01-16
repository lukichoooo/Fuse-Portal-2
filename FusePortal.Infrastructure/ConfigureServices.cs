
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FusePortal.Infrastructure.Repo;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Infrastructure.Messaging;
using FusePortal.Application.Interfaces.Messaging;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Application.Common;

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


            // auth
            services.AddHttpContextAccessor();
            services.AddScoped<IUserSecurityService, UserSecurityService>();
            services.AddScoped<ICurrentContext, CurrentContext>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEncryptor, Encryptor>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EncryptorSettings>(configuration.GetSection("EncryptorSettings"));


            // repo
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IUserRepo, UserRepo>();


            // messaging
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


            return services;
        }
    }
}
