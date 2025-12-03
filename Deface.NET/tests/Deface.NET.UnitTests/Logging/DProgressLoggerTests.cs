using Deface.NET.Logging;
using Microsoft.Extensions.Logging.Testing;

namespace Deface.NET.UnitTests.Logging;

public class DProgressLoggerTests
{
    private readonly FakeLogger<object> _logger = new();

    [Fact]
    public async Task StartAndStopShouldWorkCorrectlyToPointTimeSpan()
    {
        var dLogger = GetDProgressLogger();
        var ms = 300;
        var toleranceMs = (int)(ms * 0.9);

        dLogger.Start();
        await Task.Delay(ms);

        var timespan = dLogger.Stop();

        timespan.ShouldNotBe(TimeSpan.Zero);
        timespan.Milliseconds.ShouldBeGreaterThanOrEqualTo(toleranceMs);
    }

    [Fact]
    public void StartStop_NoDelay_LoggerShouldNotBeExecuted()
    {
        var dLogger = GetDProgressLogger();

        dLogger.Start();
        dLogger.Stop();

        _logger.Collector.Count.ShouldBe(0);
    }

    [Fact]
    public async Task LogProgress_ZeroTotalSteps_LoggerShouldNotBeExecuted()
    {
        var dLogger = GetDProgressLogger();

        dLogger.Start();
        await Task.Delay(1500);

        dLogger.Log(0, "", 0);

        dLogger.Stop();

        _logger.Collector.Count.ShouldBe(0);
    }

    [Theory]
    [InlineData(LoggingLevel.None, false)]
    [InlineData(LoggingLevel.Basic, false)]
    [InlineData(LoggingLevel.Detailed, true)]
    public async Task Log_LoggingLevelPassed_LoggedProperly(LoggingLevel loggingLevel, bool shouldLog)
    {
        var dLogger = GetDProgressLogger(loggingLevel);

        dLogger.Start();
        await Task.Delay(1250);

        dLogger.Log(1, "", 1);

        _logger.Collector.Count.ShouldBe(shouldLog ? 1 : 0);
    }

    private DProgressLogger<object> GetDProgressLogger(LoggingLevel level = LoggingLevel.Detailed)
    {
        return new(_logger, level);
    }
}
