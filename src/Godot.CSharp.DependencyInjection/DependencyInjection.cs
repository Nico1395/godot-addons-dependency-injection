using Godot.CSharp.DependencyInjection.Logging;
using Godot.CSharp.DependencyInjection.Options;
using Godot.CSharp.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection;

internal static class DependencyInjection
{
    internal static IServiceCollection AddGodotDependencyInjection(this IServiceCollection services, Action<DependencyInjectionOptions>? optionsAction = null)
    {
        var options = new DependencyInjectionOptions();
        optionsAction?.Invoke(options);

        services.AddSingleton<IDependencyInjectionOptionsProvider>(new DependencyInjectionOptionsProvider(options));
        services.AddSingleton<IInternalEditorLogger, InternalEditorLogger>();
        services.AddSingleton<IInjectionService, InjectionService>();

        return services;
    }
}
