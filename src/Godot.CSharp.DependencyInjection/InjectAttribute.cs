namespace Godot.CSharp.DependencyInjection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectAttribute : Attribute
{
    public InjectAttribute() { }

    public InjectAttribute(object key)
    {
        Key = key;
    }

    public object? Key { get; }
}
