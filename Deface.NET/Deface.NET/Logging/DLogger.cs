using Microsoft.Extensions.Logging;

namespace Deface.NET.Logging;

internal class DLogger<T>(Settings settings, ILogger<T> logger) : IDLogger<T>
{
    private readonly ILogger<T> _logger = logger;
    private readonly LoggingLevel _loggingLevel = settings.LoggingLevel;

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