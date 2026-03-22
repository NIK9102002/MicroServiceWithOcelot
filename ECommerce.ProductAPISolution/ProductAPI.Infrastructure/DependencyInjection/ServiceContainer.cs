using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Application.Interfaces;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            services.AddScoped<IProduct, ProductRepository>();            
            return services;
        }
        
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedServices(app);
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
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
