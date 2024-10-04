using Godot.CSharp.DependencyInjection;
using Godot.CSharp.DependencyInjection.Nodes;
using Godot.CSharp.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Godot.Infrastructure.DependencyInjection;

public abstract partial class DependencyInjectionNodeBase : Node
{
    [Export]
    public string? RootNode { get; private set; }

    public sealed override void _EnterTree()
    {
        if (RootNode == null)
            throw new NullReferenceException($"GDI0001 - The key/name of the scenes root node has not been specified! Setup the key/name by specifying the name of the root node for the parameter '{nameof(RootNode)}' of that scenes dependency injection node.");

        var sceneRootNode = (Engine.GetMainLoop() as SceneTree)?.CurrentScene
            ?? throw new NullReferenceException($"GDI0002 - No node with the key/name '{RootNode}' has been found. Either the key/name is not the name of the root node of the scene or the name is incorrect.");

        var flattenedNodes = sceneRootNode.FlattenNodes();
        foreach (var node in flattenedNodes)
            InjectServices(node);
    }

    private void InjectServices(Node node)
    {
        var nodeType = node.GetType();
        var propertiesToInject = nodeType.GetProperties().Where(p => p.CanWrite && p.GetCustomAttribute<InjectAttribute>() != null).ToList();

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
            return InternalServiceProviderManager.ServiceProvider.GetRequiredService(dependencyType);

        return InternalServiceProviderManager.ServiceProvider.GetRequiredKeyedService(dependencyType, key);
    }
}
