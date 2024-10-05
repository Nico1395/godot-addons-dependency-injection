using Godot.CSharp.DependencyInjection.Logging;
using Godot.CSharp.DependencyInjection.Options;
using System.Diagnostics;
using System.Reflection;

namespace Godot.CSharp.DependencyInjection.Services;

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

    public void InjectDependencies(Node node)
    {
        var stopwatch = _performanceLoggingEnabled ? Stopwatch.StartNew() : null;
        var dependencyPropertiesToInitialize = node
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<InjectAttribute>() != null)
            .ToList();

        if (dependencyPropertiesToInitialize.Count < 1)
            return;

        _editorLogger.Log($"Trying to resolve dependencies ({dependencyPropertiesToInitialize.Count}) for node '{node.Name}'...");

        foreach (var dependencyProperty in dependencyPropertiesToInitialize)
        {
            var injectAttribute = dependencyProperty.GetCustomAttribute<InjectAttribute>();
            var resolvedDependency = ResolveDependency(dependencyProperty.PropertyType, injectAttribute!.Key);

            dependencyProperty.SetValue(node, resolvedDependency, null);
        }

        if (stopwatch != null)
        {
            stopwatch.Stop();
            _editorLogger.Log($"Resolving dependencies ({dependencyPropertiesToInitialize.Count}) for node '{node.Name}' took {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
    }

    private object ResolveDependency(Type dependencyType, object? key)
    {
        if (key == null)
            return InternalServiceProvider.GetRequiredService(dependencyType);

        return InternalServiceProvider.GetRequiredKeyedService(dependencyType, key);
    }
}
