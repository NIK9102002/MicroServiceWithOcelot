using eCommerce.SharedLibrary.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Application.Mappings;
using OrderAPI.Application.Services;
using Polly;
using Polly.Retry;

namespace OrderAPI.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());                        

            services.AddHttpClient<IOrderService, OrderService>(
            client =>
            {
                client.BaseAddress = new Uri(configuration["ApiGateway:BaseAddress"]!);
                client.Timeout = TimeSpan.FromSeconds(10);
            })
            .AddResilienceHandler("retry", builder =>
            {
                builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .Handle<TaskCanceledException>()
                        .HandleResult(r => !r.IsSuccessStatusCode),

                    MaxRetryAttempts = 10,
                    Delay = TimeSpan.FromMilliseconds(500),
                    OnRetry = args =>
                    {
                        string message = $"OnRetry, Attempt: {args.AttemptNumber} Outcome: {args.Outcome}";
                        LogException.LogToConsole(message);
                        LogException.LogToDebugger(message);
                        return ValueTask.CompletedTask;
                    }
                });
            });

            return services;
        }
    }
}
