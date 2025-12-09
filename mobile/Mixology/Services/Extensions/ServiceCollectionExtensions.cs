using Microsoft.Extensions.Configuration;

namespace Mixology.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiClient<TService>(this IServiceCollection services, IConfiguration config)
        where TService : class
    {
        services.AddHttpClient<TService>(client =>
        {
            var baseUrl = config["ApiBaseUrl"];

#if ANDROID
            baseUrl = baseUrl.Replace("localhost", "10.0.2.2");
#endif
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}