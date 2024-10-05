using Godot.CSharp.DependencyInjection.Logging;
using Godot.CSharp.DependencyInjection.Options;
using Godot.CSharp.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection;

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
