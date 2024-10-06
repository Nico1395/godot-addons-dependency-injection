using GodotAddons.DependencyInjection.Logging;
using GodotAddons.DependencyInjection.Options;
using GodotAddons.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GodotAddons.DependencyInjection;

internal static class DependencyInjection
{
    internal static IServiceCollection AddGodotDependencyInjection(this IServiceCollection services, Action<DependencyInjectionOptionsBuilder>? options = null)
    {
        var optionsBuilder = new DependencyInjectionOptionsBuilder();
        options?.Invoke(optionsBuilder);

        services.AddSingleton<IDependencyInjectionOptionsProvider>(new DependencyInjectionOptionsProvider(optionsBuilder.Build()));
        services.AddSingleton<IInternalEditorLogger, InternalEditorLogger>();
        services.AddSingleton<IInjectionService, InjectionService>();

        return services;
    }
}
