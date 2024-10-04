namespace Godot.CSharp.DependencyInjection;

/// <summary>
/// Attribute flags a property to be injected when a node enters the tree.
/// </summary>
/// <remarks>
/// <para>
/// Note: This means a to-be-injected dependency is likely not available in the <see cref="Node._EnterTree"/>-method!
/// </para>
/// <para>
/// The attribute can be placed above properties with any access modifier. The property simply needs a getter and setter.
/// </para>
/// <para>
/// Supports keyed injection by specifying a <see cref="Key"/>.
/// </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectAttribute : Attribute
{
    /// <summary>
    /// Empty constructor without a key. The dependency will not be resolved using a key.
    /// </summary>
    public InjectAttribute() { }

    /// <summary>
    /// Empty constructor with a <paramref name="key"/>. The dependency will be resolved using the specified <paramref name="key"/>.
    /// </summary>
    public InjectAttribute(object key)
    {
        Key = key;
    }

    /// <summary>
    /// Key for keyed injection. If not specified the dependency will be resolved without a key.
    /// </summary>
    public object? Key { get; init; }
}
