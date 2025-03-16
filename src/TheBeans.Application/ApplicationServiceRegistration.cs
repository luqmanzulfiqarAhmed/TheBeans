using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
namespace TheBeans.Application
{

    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper registration
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // MediatR registration (new syntax)
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }

}

