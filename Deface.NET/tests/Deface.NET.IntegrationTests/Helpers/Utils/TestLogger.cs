using Microsoft.Extensions.Logging;

namespace Deface.NET.IntegrationTests.Helpers.Utils;

public class TestLogger : ILogger
{
    private readonly List<string> _messages = [];
    public IReadOnlyList<string> Messages => _messages;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        _messages.Add(message);
    }
}
