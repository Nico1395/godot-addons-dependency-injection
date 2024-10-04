using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection.Services;

internal static class InternalServiceProvider
{
    private static IServiceProvider? _serviceProvider;

    private static IServiceProvider ServiceProvider
        =>  _serviceProvider ?? throw new NullReferenceException($"The internal {nameof(IServiceProvider)} has not been initialized!");

    internal static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal static object? GetService<T>(Type serviceType)
    {
        return ServiceProvider.GetService(serviceType);
    }

    internal static T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    internal static object GetRequiredService(Type serviceType)
    {
        return ServiceProvider.GetRequiredService(serviceType);
    }

    internal static T GetRequiredService<T>()
        where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    internal static object? GetKeyedService<T>(object? serviceKey)
    {
        return ServiceProvider.GetKeyedService<T>(serviceKey);
    }

    public static object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        return ServiceProvider.GetRequiredKeyedService(serviceType, serviceKey);
    }

    public static T GetRequiredKeyedService<T>(object? serviceKey)
         where T : notnull
    {
        return ServiceProvider.GetRequiredKeyedService<T>(serviceKey);
    }
}
