using System.Reflection;
using FluentValidation;
using FusePortal.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FusePortal.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(
                this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            });

            return services;
        }
    }
}
