using Deface.NET.Logging;
using Deface.NET.UnitTests._TestsConfig;
using Microsoft.Extensions.Logging.Testing;

namespace Deface.NET.UnitTests.Logging;

[Collection(nameof(SettingsCollection))]
public class DLoggerTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    [Theory]
    [InlineData(LoggingLevel.None, LoggingLevel.Basic, false)]
    [InlineData(LoggingLevel.None, LoggingLevel.Detailed, false)]
    [InlineData(LoggingLevel.Basic, LoggingLevel.Basic, true)]
    [InlineData(LoggingLevel.Basic, LoggingLevel.Detailed, false)]
    [InlineData(LoggingLevel.Detailed, LoggingLevel.Basic, true)]
    [InlineData(LoggingLevel.Detailed, LoggingLevel.Detailed, true)]
    public void Log_LoggerIsExecutedCorrectly(LoggingLevel settingsLoggingLevel, LoggingLevel desiredLoggingLevel, bool shouldLog)
    {
        var (dLogger, logger) = GetDLogger(settingsLoggingLevel);

        dLogger.Log(desiredLoggingLevel, "whatever");

        logger.Collector.Count.ShouldBe(shouldLog ? 1 : 0);
    }

    [Theory]
    [InlineData(LoggingLevel.None, false)]
    [InlineData(LoggingLevel.Basic, true)]
    [InlineData(LoggingLevel.Detailed, true)]
    public void LogBasic_LoggerIsExecutedCorrectly(LoggingLevel settingsLoggingLevel, bool shouldLog)
    {
        var (dLogger, logger) = GetDLogger(settingsLoggingLevel);

        dLogger.LogBasic("whatever");

        logger.Collector.Count.ShouldBe(shouldLog ? 1 : 0);
    }

    [Theory]
    [InlineData(LoggingLevel.None, false)]
    [InlineData(LoggingLevel.Basic, false)]
    [InlineData(LoggingLevel.Detailed, true)]
    public void LogDetailed_LoggerIsExecutedCorrectly(LoggingLevel settingsLoggingLevel, bool shouldLog)
    {
        var (dLogger, logger) = GetDLogger(settingsLoggingLevel);

        dLogger.LogDetailed("whatever");

        logger.Collector.Count.ShouldBe(shouldLog ? 1 : 0);
    }

    [Fact]
    public void GetProgressLogger_ReturnsDProgressLogger()
    {
        var (dLogger, _) = GetDLogger(LoggingLevel.Basic);

        var progressLogger = dLogger.GetProgressLogger();

        progressLogger.ShouldNotBeNull();
    }

    private (DLogger<object>, FakeLogger<object>) GetDLogger(LoggingLevel loggingLevel)
    {
        var logger = new FakeLogger<object>();
        var settingsProvider = _settingsFixture.GetScopedSettingsProvider(x => x.LoggingLevel = loggingLevel);

        return (new(settingsProvider, logger), logger);
    }
}
