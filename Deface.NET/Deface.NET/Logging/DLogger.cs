using Deface.NET.Configuration.Provider;
using Microsoft.Extensions.Logging;

namespace Deface.NET.Logging;

internal class DLogger<T>(IScopedSettingsProvider settingsProvider, ILogger<T> logger) : IDLogger<T>
{
    private readonly ILogger<T> _logger = logger;
    private readonly LoggingLevel _loggingLevel = settingsProvider.Settings.LoggingLevel;

    public void LogBasic(string? message, params object[]? args) => Log(LoggingLevel.Basic, message, args);
    public void LogDetailed(string? message, params object[]? args) => Log(LoggingLevel.Detailed, message, args);

    public void Log(LoggingLevel loggingLevel, string? message, params object[]? args)
    {
        if (loggingLevel > _loggingLevel)
        {
            return;
        }

        _logger.LogInformation(message, args!);
    }

    public DProgressLogger<T> GetProgressLogger() => new(_logger, _loggingLevel);
}