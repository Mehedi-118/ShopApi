using System.Reflection;

using eShop.Auth.Application.Helpers;
using eShop.Auth.Infrastructure;


namespace eShop.Auth.Helpers;

public static class ConfigureServices
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = new[]
        {
            typeof(eShop.Auth.Application.IEShopAuthApplicationAssemblyMarker).Assembly,
            typeof(eShop.Auth.Infrastructure.IEShopAuthInfraAssemblyMarker).Assembly,
        };
        // Scan assemblies for types implementing InjectServices
        var injectServiceTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(Injector).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();

        // Use DI container to create and invoke RegisterServices
        var serviceProvider = services.BuildServiceProvider();
        foreach (var serviceType in injectServiceTypes)
        {
            var instance = (Injector)ActivatorUtilities.CreateInstance(serviceProvider, serviceType);
            instance.RegisterServices(services,configuration);
        }
    }
}