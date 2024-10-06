namespace GodotAddons.DependencyInjection.Logging;

/// <summary>
/// Modes that determine whether the internal logger should log or not.
/// </summary>
public enum EditorLoggingMode
{
    /// <summary>
    /// The logger is never logging.
    /// </summary>
    Never,

    /// <summary>
    /// The logger only logs during debugging.
    /// </summary>
    Debug,

    /// <summary>
    /// The logger always logs.
    /// </summary>
    Always,
}
