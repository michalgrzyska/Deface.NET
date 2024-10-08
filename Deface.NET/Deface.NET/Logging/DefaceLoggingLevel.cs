namespace Deface.NET;

/// <summary>
/// Specifies logging level for deface logs. All logs are printed with <code>logger.LogInformation</code>,
/// this enum specifies amount of printed logs.
/// </summary>
public enum DefaceLoggingLevel
{
    /// <summary>
    /// No logs printed from Deface.
    /// </summary>
    None = 0,

    /// <summary>
    /// Basic logs printed, for example information about processed video.
    /// </summary>
    Basic = 1,

    /// <summary>
    /// Detailed logging - prints live progress of items being processed.
    /// </summary>
    Detailed = 2
}
