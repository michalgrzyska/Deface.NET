using Deface.NET.CommercialFeatures;
using Deface.NET.CommercialFeatures.Interfaces;
using Deface.NET.Common;
using Deface.NET.Configuration.Provider;
using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Configuration.Validation;
using Deface.NET.Graphics;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.ObjectDetection.ONNX;
using Deface.NET.ObjectDetection.YoloNasLicensePlates;
using Deface.NET.Processing;
using Deface.NET.System;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO;
using Deface.NET.VideoIO.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Deface.NET;

/// <summary>
/// Dependency injection extension for Deface
/// </summary>
[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    /// <summary>
    /// Adds Deface module to DI container
    /// </summary>
    public static IServiceCollection AddDeface(this IServiceCollection services, Action<Settings>? builder)
    {
        services.AddSingleton<IObjectDetector, ObjectDetector>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<IFrameCreator, FrameCreator>();
        services.AddSingleton<ILicensePlateDetector, LicensePlateDetector>();
        services.AddSingleton<IExternalProcessFactory, ExternalProcessFactory>();
        services.AddSingleton<IOnnxProvider, OnnxProvider>();
        services.AddSingleton<ISettingsValidator, SettingsValidator>();
        services.AddSingleton<IAppFiles, AppFiles>();

        services.AddSettingsProvider(builder);

        services.AddScoped<IVideoProcessor, VideoProcessor>();
        services.AddScoped<IImageProcessor, ImageProcessor>();
        services.AddScoped<IVideoWriter, VideoWriter>();
        services.AddScoped<IVideoReader, VideoReader>();
        services.AddScoped<IVideoInfoProvider, VideoInfoProvider>();
        services.AddScoped<IShapeDrawerProvider, ShapeDrawerProvider>();
        services.AddScoped<IVideoEncoderChecker, VideoEncoderChecker>();
        services.AddScoped<ICommercialFeaturesReporter, CommercialFeaturesReporter>();
        services.AddScoped<IScopedSettingsProvider, ScopedSettingsProvider>();

        services.AddSingleton<IDefaceService, DefaceService>();
        services.AddTransient(typeof(IDLogger<>), typeof(DLogger<>));
        services.AddDefaultLoggerIfNeeded();

        return services;
    }

    private static void AddSettingsProvider(this IServiceCollection services, Action<Settings>? builder)
    {
        services.AddSingleton<ISettingsProvider, SettingsProvider>();

        using var tempProvider = services.BuildServiceProvider();
        var settingsProvider = tempProvider.GetRequiredService<ISettingsProvider>();

        settingsProvider.Initialize(builder);

        services.AddSingleton(settingsProvider);
    }
}
