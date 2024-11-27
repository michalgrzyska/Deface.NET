using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.System;
using System.Diagnostics;

namespace Deface.NET.Processing;

internal sealed class ImageProcessor
(
    IScopedSettingsProvider settingsProvider,
    IDLogger<IDefaceService> logger,
    IObjectDetector detector,
    IShapeDrawerProvider shapeDrawerProvider,
    IFileSystem fileSystem,
    IFrameCreator frameCreator
) : IImageProcessor
{
    private readonly IDLogger<IDefaceService> _logger = logger;
    private readonly IObjectDetector _detector = detector;
    private readonly IShapeDrawerProvider _shapeDrawerProvider = shapeDrawerProvider;
    private readonly IFileSystem _fileSystem = fileSystem;
    private readonly IFrameCreator _frameCreator = frameCreator;

    private readonly Settings _settings = settingsProvider.Settings;

    private readonly static string[] ImageExtensions = [".jpg", ".jpeg", ".png"];

    public ProcessingResult Process(string inputPath, string outputPath)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        using Frame image = _frameCreator.FromFile(inputPath);
        ProcessImage(image, outputPath);

        stopwatch.Stop();

        _logger.Log(LoggingLevel.Basic, "Processed {Input} and saved to {Output}", inputPath, outputPath);
        return new(inputPath, outputPath, stopwatch.Elapsed, _settings.Threshold, 1);
    }

    public List<ProcessingResult> ProcessMany(string inputDirectory, string outputDirectory)
    {
        var filenames = _fileSystem.GetFiles(inputDirectory);

        var imageFilenames = filenames
            .Where(file => ImageExtensions.Contains(Path.GetExtension(file)))
            .ToArray();

        List<ProcessingResult> results = [];

        foreach (var imageFilename in imageFilenames)
        {
            var outputPath = GetOutputPath(imageFilename, outputDirectory);
            var result = Process(imageFilename, outputPath);

            results.Add(result);
        }

        return results;
    }

    private void ProcessImage(Frame image, string outputPath)
    {
        List<DetectedObject> detectedObjects = _detector.Detect(image, _settings);
        using Frame result = _shapeDrawerProvider.ShapeDrawer.Draw(image, detectedObjects);

        var resultBytes = result.ToByteArray(_settings.ImageFormat);
        _fileSystem.Save(outputPath, resultBytes);
    }

    private static string GetOutputPath(string inputPath, string outputDirPath)
    {
        var filename = Path.GetFileName(inputPath);
        return Path.Combine(outputDirPath, filename);
    }
}
