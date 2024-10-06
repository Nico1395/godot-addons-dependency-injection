namespace GodotAddons.DependencyInjection.Options;

internal sealed class DependencyInjectionOptionsProvider : IDependencyInjectionOptionsProvider
{
    private readonly DependencyInjectionOptions _options;

    internal DependencyInjectionOptionsProvider(DependencyInjectionOptions options)
    {
        _options = options;
    }

    public DependencyInjectionOptions GetOptions()
    {
        return _options;
    }
}
