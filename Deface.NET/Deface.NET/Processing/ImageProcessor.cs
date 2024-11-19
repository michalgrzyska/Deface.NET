using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using System.Diagnostics;

namespace Deface.NET.Processing;

internal sealed class ImageProcessor
(
    IScopedSettingsProvider settingsProvider,
    IDLogger<IDefaceService> logger,
    IObjectDetector detector,
    IShapeDrawer shapeDrawingService,
    FileSystem fileSystem,
    IFrameCreator frameCreator
) : IDisposable
{
    private readonly IDLogger<IDefaceService> _logger = logger;
    private readonly IObjectDetector _detector = detector;
    private readonly IShapeDrawer _shapeDrawingService = shapeDrawingService;
    private readonly FileSystem _fileSystem = fileSystem;
    private readonly IFrameCreator _frameCreator = frameCreator;
    private readonly Settings _settings = settingsProvider.Settings;

    private readonly static string[] ImageExtensions = [".jpg", ".jpeg", ".png"];

    public void Dispose() => _detector.Dispose();

    public ProcessingResult Process(string inputPath, string outputPath)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        using Frame image = _frameCreator.FromFile(inputPath);
        ProcessImage(image, outputPath);

        stopwatch.Stop();

        _logger.Log(DefaceLoggingLevel.Basic, "Processed {Input} and saved to {Output}", inputPath, outputPath);
        return new(inputPath, outputPath, stopwatch.Elapsed, _settings.Threshold, 1);
    }

    public List<ProcessingResult> ProcessMany(string inputDirectory, string outputDirectory)
    {
        var imageFilenames = Directory
            .GetFiles(inputDirectory)
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
        Frame result = _shapeDrawingService.DrawShapes(image, detectedObjects);

        var resultBytes = result.ToByteArray(_settings.ImageFormat);
        _fileSystem.Save(outputPath, resultBytes);
    }

    private static string GetOutputPath(string inputPath, string outputDirPath)
    {
        var filename = Path.GetFileName(inputPath);
        return Path.Combine(outputDirPath, filename);
    }
}
