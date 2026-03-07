using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Application.Interfaces;
using OrderAPI.Infrastructure.Data;
using OrderAPI.Infrastructure.Repositories;

namespace OrderAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, configuration, configuration["MySerilog:FileName"]!);

            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedServices(app);
            return app;
        }
    }
}
