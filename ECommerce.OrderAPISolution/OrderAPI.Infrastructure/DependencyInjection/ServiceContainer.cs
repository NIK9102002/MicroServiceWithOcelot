using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
                int retries = 10;

                while (retries > 0)
                {
                    try
                    {
                        dbContext.Database.Migrate();
                        Console.WriteLine($"Migrated Successfully");
                        break;
                    }
                    catch (Exception ex)
                    {
                        retries--;
                        Console.WriteLine($"DB not ready. Retrying... {retries} left");
                        Thread.Sleep(5000);
                    }
                }
            }
            return app;
        }
    }
}
