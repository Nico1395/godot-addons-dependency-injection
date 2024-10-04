namespace Godot.CSharp.DependencyInjection.Logging;

internal interface IInternalEditorLogger
{
    public void Log(string message);
    public void Log(Exception exception);
}
