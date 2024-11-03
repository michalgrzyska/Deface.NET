using Deface.NET.Graphics;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.ObjectDetection.UltraFace;
using System.Diagnostics;

namespace Deface.NET.Processing;

internal sealed class ImageProcessor(Settings settings, DLogger<IDefaceService> logger) : ProcessorBase(settings), IDisposable
{
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly UltraFaceDetector _detector = new();

    private readonly static string[] ImageExtensions = [".jpg", ".jpeg", ".png"];

    public void Dispose() => _detector.Dispose();

    public ProcessingResult Process(string inputPath, string outputPath, Action<Settings>? customSettings)
    {
        ApplyScopedSettings(customSettings);

        Stopwatch stopwatch = new();
        stopwatch.Start();

        using Frame image = LoadImage(inputPath);
        ProcessImage(image, outputPath);

        stopwatch.Stop();

        _logger.Log(DefaceLoggingLevel.Basic, "Processed {Input} and saved to {Output}", inputPath, outputPath);
        return new(inputPath, outputPath, stopwatch.Elapsed, Settings.Threshold, 1);
    }

    public List<ProcessingResult> ProcessMany(string inputDirectory, string outputDirectory, Action<Settings>? customSettings)
    {
        ApplyScopedSettings(customSettings);

        var imageFilenames = Directory
            .GetFiles(inputDirectory)
            .Where(file => ImageExtensions.Contains(Path.GetExtension(file)))
            .ToArray();

        List<ProcessingResult> results = [];

        foreach (var imageFilename in imageFilenames)
        {
            var outputPath = GetOutputPath(imageFilename, outputDirectory);
            var result = Process(imageFilename, outputPath, default); // We don't pass settings so we don't apply them in loop

            results.Add(result);
        }

        return results;
    }

    private static Frame LoadImage(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Image file not found", path);
            }

            using FileStream stream = File.OpenRead(path);
            return new(stream);
        }
        catch (Exception e)
        {
            throw new DefaceException($"Could not open image {path}.", e);
        }
    }

    private void ProcessImage(Frame image, string outputPath)
    {
        List<DetectedObject> detectedObjects = _detector.Detect(image);
        Frame result = ShapeDrawer.DrawShapes(image, detectedObjects, Settings);

        result.SaveTo(outputPath);
    }

    private static string GetOutputPath(string inputPath, string outputDirPath)
    {
        var filename = Path.GetFileName(inputPath);
        return Path.Combine(outputDirPath, filename);
    }
}
