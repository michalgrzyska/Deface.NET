using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing;
using Deface.NET.System;
using Deface.NET.VideoIO;
using Deface.NET.VideoIO.Interfaces;
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
        services.AddScoped<IScopedSettingsProvider, ScopedSettingsProvider>();

        services.AddSingleton<IDefaceService, DefaceService>();
        services.AddTransient(typeof(IDLogger<>), typeof(DLogger<>));
        services.AddDefaultLoggerIfNeeded();

        services.AddScoped<VideoProcessor>();
        services.AddScoped<ImageProcessor>();
        services.AddScoped<IVideoWriter, VideoWriter>();
        services.AddScoped<IVideoReader, VideoReader>();
        services.AddScoped<IVideoInfoProvider, VideoInfoProvider>();
        services.AddScoped<IShapeDrawer, ShapeDrawer>();

        services.AddSingleton<IObjectDetector, ObjectDetector>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<IFrameCreator, FrameCreator>();

        return services;
    }
}
