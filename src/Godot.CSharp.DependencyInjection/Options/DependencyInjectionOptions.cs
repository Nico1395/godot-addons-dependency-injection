using Godot.CSharp.DependencyInjection.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection.Options;

public sealed class DependencyInjectionOptions
{
    public ServiceProviderOptions ServiceProviderOptions { get; set; } = new ServiceProviderOptions();
    public EditorLoggingMode EditorLoggingMode { get; set; } = EditorLoggingMode.None;
}
