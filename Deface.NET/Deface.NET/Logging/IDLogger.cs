namespace Deface.NET.Logging;

internal interface IDLogger<T>
{
    DProgressLogger<T> GetProgressLogger();
    void Log(LoggingLevel loggingLevel, string? message, params object[]? args);
}
