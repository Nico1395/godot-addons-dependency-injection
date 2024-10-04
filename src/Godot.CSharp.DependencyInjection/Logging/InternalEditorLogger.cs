using Godot.CSharp.DependencyInjection.Options;
using System.Diagnostics;

namespace Godot.CSharp.DependencyInjection.Logging;

internal sealed class InternalEditorLogger : IInternalEditorLogger
{
    private readonly IDependencyInjectionOptionsProvider _optionsProvider;

    public InternalEditorLogger(IDependencyInjectionOptionsProvider optionsProvider)
    {
        _optionsProvider = optionsProvider;
    }

    public void Log(string message)
    {
        if (!ShouldLog())
            return;

        var formattedMessage = FormatMessage(message);

        GD.Print(formattedMessage);
        Debug.WriteLine(formattedMessage);
    }

    public void Log(Exception exception)
    {
        Log(exception.ToString());
    }

    private bool ShouldLog()
    {
        return _optionsProvider.GetOptions().EditorLoggingMode switch
        {
            EditorLoggingMode.Debug => IsDebug(),
            EditorLoggingMode.Always => true,
            EditorLoggingMode.Never or _ => false,
        };
    }

    private bool IsDebug()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }

    private string FormatMessage(string message)
    {
        return $"{DateTime.Now} - {message}";
    }
}
