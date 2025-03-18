using System.Reflection;
using FluentValidation;
using MediatR;
namespace TheBeans.Api.Extensions;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Add controller support
            services.AddControllers();

            
            return services;
        }
    }