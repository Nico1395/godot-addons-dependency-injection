using Godot.CSharp.DependencyInjection;
using Godot.CSharp.DependencyInjection.Logging;
using Godot.CSharp.DependencyInjection.Options;
using Godot.CSharp.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Godot.Infrastructure.DependencyInjection;

public abstract partial class DependencyInjectionStartupNodeBase : Node
{
    public override void _EnterTree()
    {
        var options = new DependencyInjectionOptions();
        ConfigureOptions(options);
        var optionsProvider = new DependencyInjectionOptionsProvider(options);

        var services = new ServiceCollection();

        services.AddSingleton<IDependencyInjectionOptionsProvider>(optionsProvider);
        services.AddSingleton<IInternalEditorLogger, InternalEditorLogger>();

        ConfigureDependencies(services);
        InternalServiceProviderManager.ServiceProvider = services.BuildServiceProvider();

        GD.Print($"Registered '{services.Count}' services to the {nameof(IServiceCollection)}.");

        try
        {
            Engine.GetMainLoop().Connect(
                new StringName("node_added"),
                Callable.From<Node>(OnNodeAdded));
        }
        catch (Exception ex)
        {
            GD.Print(ex.ToString());
        }
    }

    protected virtual void ConfigureOptions(DependencyInjectionOptions options)
    {
    }

    protected abstract void ConfigureDependencies(IServiceCollection services);

    private void OnNodeAdded(Node node)
    {
        InjectServices(node);
    }

    private void InjectServices(Node node)
    {
        var nodeType = node.GetType();
        var propertiesToInject = nodeType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => p.GetCustomAttribute<InjectAttribute>() != null).ToList();

        GD.Print($"Trying to resolve {propertiesToInject.Count} dependencies for node {node.Name}...");

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
