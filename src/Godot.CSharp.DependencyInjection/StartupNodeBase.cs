using Godot.CSharp.DependencyInjection.Logging;
using Godot.CSharp.DependencyInjection.Options;
using Godot.CSharp.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Godot.CSharp.DependencyInjection;

/// <summary>
/// Base class for the dependency injection startup node. Allows registering services with in internal <see cref="IServiceCollection"/> by overriding the method <see cref="ConfigureDependencies(IServiceCollection)"/>.
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
public abstract partial class StartupNodeBase : Node
{
    /// <inheritdoc/>
    public sealed override void _EnterTree()
    {
        OrchestrateStartup();
        AttachToNodesBeingAddedToTheSceneTree();
        OnAfterEnterTree();
    }

    /// <summary>
    /// Method allows configuring the given <see cref="DependencyInjectionOptions"/>.
    /// </summary>
    /// <param name="options">Options to be configured.</param>
    protected virtual void ConfigureOptions(DependencyInjectionOptions options)
    {
    }

    /// <summary>
    /// Method is being invoked at the end of the node entering the scene tree, so pretty much after the startup has been orchestrated.
    /// </summary>
    protected virtual void OnAfterEnterTree()
    {
    }

    /// <summary>
    /// Method allows configuring dependencies for the application using the given <see cref="IServiceCollection"/> <paramref name="services"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> to contain registered <see cref="ServiceDescriptor"/>s.</param>
    protected virtual void ConfigureDependencies(IServiceCollection services)
    {
    }

    private void OrchestrateStartup()
    {
        var services = new ServiceCollection().AddGodotDependencyInjection(ConfigureOptions);
        ConfigureDependencies(services);
        InternalServiceProvider.SetServiceProvider(services.BuildServiceProvider());

        var editorLogger = InternalServiceProvider.GetRequiredService<IInternalEditorLogger>();
        editorLogger.Log($"Registered '{services.Count}' services to the {nameof(IServiceCollection)}.");
    }

    private void AttachToNodesBeingAddedToTheSceneTree()
    {
        var editorLogger = InternalServiceProvider.GetRequiredService<IInternalEditorLogger>();
        var injectionService = InternalServiceProvider.GetRequiredService<IInjectionService>();

        try
        {
            Engine.GetMainLoop().Connect(
                new StringName("node_added"),
                Callable.From<Node>(injectionService.InjectDependencies));
        }
        catch (Exception ex)
        {
            editorLogger.Log(ex.ToString());
        }
    }
}
