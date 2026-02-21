using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Application.Mappings;

namespace ProductAPI.Application.DependenyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
            return services;
        }        
    }
}
