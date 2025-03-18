using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheBeans.Core.Interfaces.Repositories;
using TheBeans.Core.Interfaces.Services;
using TheBeans.Infrastructure.Data;
using TheBeans.Infrastructure.Repositories;
using TheBeans.Infrastructure.Services;

namespace TheBeans.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the DbContext using PostgreSQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Register generic repository implementations.
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));

            // Register domain services.
            services.AddScoped<IDailyBeanService, DailyBeanService>();

            return services;
        }
    }
}