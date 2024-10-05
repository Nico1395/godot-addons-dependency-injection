using Godot.CSharp.DependencyInjection.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection.Options;

/// <summary>
/// Builder implementation for <see cref="DependencyInjectionOptions"/>
/// </summary>
public sealed class DependencyInjectionOptionsBuilder
{
    private readonly DependencyInjectionOptions _options = new();

    /// <summary>
    /// Method enables performance logging.
    /// </summary>
    /// <remarks>
    /// Only logs if the <see cref="EditorLoggingMode"/> is configured to allow logging into the editor.
    /// </remarks>
    /// <returns><see cref="DependencyInjectionOptionsBuilder"/> for further configurations.</returns>
    public DependencyInjectionOptionsBuilder WithPerformanceLoggingEnabled()
    {
        _options.EnablePerformanceLogging = true;
        return this;
    }

    /// <summary>
    /// Method sets the <see cref="EditorLoggingMode"/> for internal logging on dependency injection related events.
    /// </summary>
    /// <param name="mode">Specified <see cref="EditorLoggingMode"/> to be set.</param>
    /// <returns><see cref="DependencyInjectionOptionsBuilder"/> for further configurations.</returns>
    public DependencyInjectionOptionsBuilder WithEditorLoggingMode(EditorLoggingMode mode)
    {
        _options.EditorLoggingMode = mode;
        return this;
    }

    /// <summary>
    /// Method allows setting <see cref="ServiceProviderOptions"/> that are used when building an <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceProviderOptions"><see cref="ServiceProviderOptions"/> being set.</param>
    /// <returns><see cref="DependencyInjectionOptionsBuilder"/> for further configurations.</returns>
    public DependencyInjectionOptionsBuilder WithServiceProviderOptions(ServiceProviderOptions serviceProviderOptions)
    {
        _options.ServiceProviderOptions = serviceProviderOptions;
        return this;
    }

    /// <summary>
    /// Method allows configuring <see cref="ServiceProviderOptions"/> that are used when building an <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="serviceProviderOptions"><see cref="Action{ServiceProviderOptions}"/> allowing to configure <see cref="ServiceProviderOptions"/>.</param>
    /// <returns><see cref="DependencyInjectionOptionsBuilder"/> for further configurations.</returns>
    public DependencyInjectionOptionsBuilder WithServiceProviderOptions(Action<ServiceProviderOptions> serviceProviderOptions)
    {
        serviceProviderOptions.Invoke(_options.ServiceProviderOptions);
        return this;
    }

    internal DependencyInjectionOptions Build()
    {
        return _options;
    }
}
