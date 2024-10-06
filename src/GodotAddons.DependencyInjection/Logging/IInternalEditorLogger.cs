namespace GodotAddons.DependencyInjection.Logging;

internal interface IInternalEditorLogger
{
    public void Log(string message);
    public void Log(Exception exception);
}
