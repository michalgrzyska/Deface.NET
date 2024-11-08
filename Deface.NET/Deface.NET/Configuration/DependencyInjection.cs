using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing;
using Deface.NET.Utils;
using Deface.NET.VideoIO;
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
        Platform platform = PlatformChecker.GetPlatform();
        Settings settings = new(builder, platform);

        services.AddSingleton(settings);
        services.AddScoped(typeof(ScopedSettingsProvider));

        services.AddSingleton<IDefaceService, DefaceService>();
        services.AddTransient(typeof(DLogger<>));
        services.AddDefaultLoggerIfNeeded();

        services.AddTransient<VideoProcessor>();
        services.AddTransient<ImageProcessor>();
        services.AddTransient<VideoWriterService>();
        services.AddTransient<VideoReaderService>();
        services.AddTransient<VideoInfoService>();
        services.AddTransient<ShapeDrawingService>();

        services.AddSingleton<ObjectDetector>();

        return services;
    }
}
