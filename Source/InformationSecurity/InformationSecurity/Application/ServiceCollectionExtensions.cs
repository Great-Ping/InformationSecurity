using InformationSecurity.Pages.Cryptography;
using InformationSecurity.Widgets.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace InformationSecurity.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<CryptographyPageModel>();
        return services;
    }
}