namespace Godot.CSharp.DependencyInjection.Services;

internal static class InternalServiceProviderManager
{
    private static IServiceProvider? _serviceProvider;

    internal static IServiceProvider ServiceProvider
    {
        get => _serviceProvider ?? throw new NullReferenceException($"The internal {nameof(IServiceProvider)} has not been initialized!");
        set => _serviceProvider = value;
    }
}
