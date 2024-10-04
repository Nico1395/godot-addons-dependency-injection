using Godot.CSharp.DependencyInjection.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection.Options;

/// <summary>
/// Options for configuring dependency injection.
/// </summary>
public sealed class DependencyInjectionOptions
{
    /// <summary>
    /// Options for the service provider that is being built after registration of services.
    /// </summary>
    public ServiceProviderOptions ServiceProviderOptions { get; set; } = new ServiceProviderOptions();

    /// <summary>
    /// Logging mode for internal dependency injection related logging operations.
    /// </summary>
    public EditorLoggingMode EditorLoggingMode { get; set; } = EditorLoggingMode.Never;
}
