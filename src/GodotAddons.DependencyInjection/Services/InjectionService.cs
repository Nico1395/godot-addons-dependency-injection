using GodotAddons.DependencyInjection.Logging;
using GodotAddons.DependencyInjection.Options;
using System.Diagnostics;
using System.Reflection;

namespace GodotAddons.DependencyInjection.Services;

internal sealed class InjectionService : IInjectionService
{
    private readonly IInternalEditorLogger _editorLogger;
    private readonly IDependencyInjectionOptionsProvider _optionsProvider;

    private readonly bool _performanceLoggingEnabled;

    public InjectionService(
        IInternalEditorLogger editorLogger,
        IDependencyInjectionOptionsProvider optionsProvider)
    {
        _editorLogger = editorLogger;
        _optionsProvider = optionsProvider;

        _performanceLoggingEnabled = _optionsProvider.GetOptions().EnablePerformanceLogging;
    }

    public void InjectDependencies(object @object)
    {
        var stopwatch = _performanceLoggingEnabled ? Stopwatch.StartNew() : null;
        var hasInjected = InjectDependenciesInternal(@object);

        if (hasInjected && stopwatch != null)
        {
            stopwatch.Stop();
            _editorLogger.Log($"Resolving dependencies for node {@object.GetType().Name} took {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
        else
        {
            stopwatch?.Stop();
        }
    }

    private bool InjectDependenciesInternal(object @object)
    {
        var objectType = @object.GetType();
        var dependencyPropertiesToInitialize = objectType
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<InjectAttribute>() != null)
            .ToList();

        if (dependencyPropertiesToInitialize.Count < 1)
            return false;

        _editorLogger.Log($"Trying to resolve dependencies ({dependencyPropertiesToInitialize.Count}) for object '{objectType.Name}'...");

        foreach (var dependencyProperty in dependencyPropertiesToInitialize)
        {
            var injectAttribute = dependencyProperty.GetCustomAttribute<InjectAttribute>();
            var resolvedDependency = ResolveDependency(dependencyProperty.PropertyType, injectAttribute!.Key);

            dependencyProperty.SetValue(@object, resolvedDependency, null);
        }

        return true;
    }

    private object ResolveDependency(Type dependencyType, object? key)
    {
        if (key == null)
            return InternalServiceProvider.GetRequiredService(dependencyType);

        return InternalServiceProvider.GetRequiredKeyedService(dependencyType, key);
    }
}
