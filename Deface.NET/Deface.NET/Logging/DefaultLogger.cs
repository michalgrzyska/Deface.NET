using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET.Logging;

[ExcludeFromCodeCoverage]
internal class DefaultLogger<T> : ILogger<T>
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        Console.WriteLine($"{message}");
    }
}

[ExcludeFromCodeCoverage]
internal static class LoggerDependencyInjection
{
    public static void AddDefaultLoggerIfNeeded(this IServiceCollection services)
    {
        var hasLoggerFactory = services.Any(descriptor => descriptor.ServiceType == typeof(ILogger<>));

        if (!hasLoggerFactory)
        {
            services.AddTransient(typeof(ILogger<>), typeof(DefaultLogger<>));
        }
    }
}