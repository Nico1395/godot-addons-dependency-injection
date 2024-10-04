using Godot.CSharp.DependencyInjection;
using Godot.CSharp.DependencyInjection.Logging;
using Godot.CSharp.DependencyInjection.Options;
using Godot.CSharp.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Godot.Infrastructure.DependencyInjection;

/// <summary>
/// Base class for the dependency injection startup node. Allows registering services with in internal <see cref="IServiceCollection"/> by implementing the method <see cref="ConfigureDependencies(IServiceCollection)"/>.
/// </summary>
/// <remarks>
/// <para>
/// Instructions: The script containing the node inheriting off of this class needs to be added as a script under '<c>Project/Project Settings/Globals/Autoload</c>'. <b>This is mandatory!</b> and ensures the node is being
/// added to the scene tree at startup, before rendering anything. Whether this script should be loaded before other auto load scripts, obvioulsy depends on your application. However if you want to use dependency injection
/// in other scripts, you will need this to be loaded earlier.
/// </para>
/// <para>
/// <see cref="DependencyInjectionOptions"/> can be configured overriding <see cref="ConfigureOptions(DependencyInjectionOptions)"/>.
/// </para>
/// </remarks>
public abstract partial class DependencyInjectionStartupNodeBase : Node
{
    /// <inheritdoc/>
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

    /// <summary>
    /// Method allows configuring the given <see cref="DependencyInjectionOptions"/>.
    /// </summary>
    /// <param name="options">Options to be configured.</param>
    protected virtual void ConfigureOptions(DependencyInjectionOptions options)
    {
    }

    /// <summary>
    /// Method allows configuring dependencies for the application using the given <see cref="IServiceCollection"/> <paramref name="services"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> to contain registered <see cref="ServiceDescriptor"/>s.</param>
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
