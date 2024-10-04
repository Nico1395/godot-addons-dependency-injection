namespace Godot.CSharp.DependencyInjection.Options;

/// <summary>
/// Provider for providing <see cref="DependencyInjectionOptions"/>.
/// </summary>
public interface IDependencyInjectionOptionsProvider
{
    /// <summary>
    /// Method provides an instance of <see cref="DependencyInjectionOptions"/>.
    /// </summary>
    /// <returns>Options configured at startup.</returns>
    public DependencyInjectionOptions GetOptions();
}
