
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Repo;

namespace FusePortal.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(
                this IServiceCollection services,
                IConfiguration configuratoin)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuratoin.GetConnectionString("AppDbContext")));

            services.AddScoped<IUserRepo, UserRepo>();

            return services;
        }
    }
}
