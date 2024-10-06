using GodotAddons.DependencyInjection.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace GodotAddons.DependencyInjection.Options;

/// <summary>
/// Options for configuring dependency injection.
/// </summary>
public sealed class DependencyInjectionOptions
{
    internal DependencyInjectionOptions() { }

    /// <summary>
    /// Options for the service provider that is being built after registration of services.
    /// </summary>
    public ServiceProviderOptions ServiceProviderOptions { get; internal set; } = new ServiceProviderOptions();

    /// <summary>
    /// Logging mode for internal dependency injection related logging operations.
    /// </summary>
    public EditorLoggingMode EditorLoggingMode { get; internal set; } = EditorLoggingMode.Never;

    /// <summary>
    /// Determines whether to enable performance logging or not.
    /// </summary>
    /// <remarks>
    /// Only logs if the <see cref="EditorLoggingMode"/> is configured to allow logging into the editor.
    /// </remarks>
    public bool EnablePerformanceLogging { get; internal set; }
}
