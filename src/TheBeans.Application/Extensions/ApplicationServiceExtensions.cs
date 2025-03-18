using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;


namespace TheBeans.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services
    )
    {
            // Register MediatR using the new API for version 12
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    new Assembly[]
                    {
                        Assembly.GetExecutingAssembly(),
                        typeof(TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean.CreateCoffeeBeanCommandHandler).Assembly
                    });
            });

            // Register AutoMapper profiles from the Application layer.
            services.AddAutoMapper(typeof(TheBeans.Application.Common.Mappings.CoffeeBeanProfile).Assembly);

            // Register FluentValidation validators from the Application layer.
            services.AddValidatorsFromAssembly(typeof(TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean.CreateCoffeeBeanCommandValidator).Assembly);


        // Add MediatR pipelines (e.g., validation, logging)
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));




        return services;
    }
}


