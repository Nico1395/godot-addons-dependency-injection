using Godot.CSharp.DependencyInjection.Logging;
using System.Reflection;

namespace Godot.CSharp.DependencyInjection.Services;

internal sealed class InjectionService : IInjectionService
{
    private readonly IInternalEditorLogger _editorLogger;

    public InjectionService(IInternalEditorLogger editorLogger)
    {
        _editorLogger = editorLogger;
    }

    public void InjectDependencies(Node node)
    {
        var nodeType = node.GetType();
        var propertiesToInject = nodeType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetCustomAttribute<InjectAttribute>() != null).ToList();

        _editorLogger.Log($"Trying to resolve {propertiesToInject.Count} dependencies for node {node.Name}...");

        foreach (var property in propertiesToInject)
        {
            var injectAttribute = property.GetCustomAttribute<InjectAttribute>();
            var resolvedDependency = ResolveDependency(property.PropertyType, injectAttribute!.Key);

            property.SetValue(node, resolvedDependency, null);
        }
    }

    private object ResolveDependency(Type dependencyType, object? key)
    {
        if (key == null)
            return InternalServiceProvider.GetRequiredService(dependencyType);

        return InternalServiceProvider.GetRequiredKeyedService(dependencyType, key);
    }
}
