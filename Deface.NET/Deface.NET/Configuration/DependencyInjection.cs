using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing;
using Microsoft.Extensions.DependencyInjection;

namespace Deface.NET;

/// <summary>
/// Dependency injection extension for Deface
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Deface module to DI container
    /// </summary>
    public static IServiceCollection AddDeface(this IServiceCollection services, Action<Settings> builder)
    {
        Settings settings = new(builder);

        services.AddSingleton(settings);
        services.AddSingleton<IDefaceService, DefaceService>();
        services.AddTransient(typeof(DLogger<>));
        services.AddDefaultLoggerIfNeeded();
        services.AddTransient<VideoProcessor>();
        services.AddTransient<ImageProcessor>();
        services.AddSingleton<ObjectDetector>();

        return services;
    }
}
