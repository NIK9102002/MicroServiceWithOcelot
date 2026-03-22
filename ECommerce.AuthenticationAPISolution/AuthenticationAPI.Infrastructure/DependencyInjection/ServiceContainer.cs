using AuthenticationAPI.Application.Interfaces;
using AuthenticationAPI.Infrastructure.Data;
using AuthenticationAPI.Infrastructure.Repositories;
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(services, config, config["MySerilog:FileName"]!);

            services.AddScoped<IUser, UserRepository>();
            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            SharedServiceContainer.UseSharedServices(app);
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
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
