using Deface.NET.Logging;
using Microsoft.Extensions.Logging.Testing;
using NSubstitute;

namespace Deface.NET.UnitTests.Logging;

public class DProgressLoggerUnitTests
{
    private readonly FakeLogger<object> _logger = new();

    [Fact]
    public async Task StartAndStopShouldWorkCorrectlyToPointTimeSpan()
    {
        var dLogger = GetDProgressLogger();

        dLogger.Start();
        await Task.Delay(300);

        var timespan = dLogger.Stop();

        timespan.Should().NotBe(TimeSpan.Zero);
        timespan.Milliseconds.Should().BeGreaterThanOrEqualTo(300);
    }

    [Theory]
    [InlineData(LoggingLevel.None, false)]
    [InlineData(LoggingLevel.Basic, false)]
    [InlineData(LoggingLevel.Detailed, true)]
    public async Task Log_LoggingLevelPassed_LoggedProperly(LoggingLevel loggingLevel, bool shouldLog)
    {
        var dLogger = GetDProgressLogger(loggingLevel);

        dLogger.Start();
        await Task.Delay(1000);

        dLogger.Log(1, "", 1);

        _logger.Collector.Count.Should().Be(shouldLog ? 1 : 0);
    }

    private DProgressLogger<object> GetDProgressLogger(LoggingLevel level = LoggingLevel.Detailed)
    {
        return new (_logger, level);
    }
}
