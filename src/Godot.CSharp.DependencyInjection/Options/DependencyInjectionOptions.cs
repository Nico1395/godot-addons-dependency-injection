using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection.Options;

public sealed class DependencyInjectionOptions
{
    public ServiceProviderOptions ServiceProviderOptions { get; set; } = new ServiceProviderOptions();
    public bool LogEventsToEditor { get; set; }
}
