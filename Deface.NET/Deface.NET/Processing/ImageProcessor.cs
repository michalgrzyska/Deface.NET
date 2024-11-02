using Deface.NET.CenterFace;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.ObjectDetection.UltraFace;
using SkiaSharp;
using System.Diagnostics;

namespace Deface.NET.Processing;

internal sealed class ImageProcessor(Settings settings, DLogger<IDefaceService> logger) : ProcessorBase(settings), IDisposable
{
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly CenterFaceModel _centerFace = new();
    private readonly UltraFaceDetector _detector = new();

    private readonly static string[] ImageExtensions = [".jpg", ".jpeg", ".png"];

    public void Dispose() => _centerFace.Dispose();

    public ProcessingResult Process(string inputPath, string outputPath, Action<Settings>? customSettings)
    {
        ApplyScopedSettings(customSettings);

        Stopwatch stopwatch = new();
        stopwatch.Start();

        using SKBitmap image = LoadImage(inputPath);
        Dimensions dimensions = ProcessImage(image, outputPath);

        stopwatch.Stop();

        _logger.Log(DefaceLoggingLevel.Basic, "Processed {Input} and saved to {Output}", inputPath, outputPath);
        return new(inputPath, outputPath, stopwatch.Elapsed, dimensions, dimensions, Settings.Threshold, 1);
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

    private static SKBitmap LoadImage(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Image file not found", path);
            }

            using FileStream stream = File.OpenRead(path);
            return SKBitmap.Decode(stream);
        }
        catch (Exception e)
        {
            throw new DefaceException($"Could not open image {path}.", e);
        }
    }

    private Dimensions ProcessImage(SKBitmap image, string outputPath)
    {
        Dimensions dimensions = new(image.Width, image.Height);
        List<DetectedObject> detectedObjects = _detector.Detect(image);
        SKBitmap result = Graphics.ShapeDrawer.DrawShapes(image, detectedObjects, Settings);

        using SKImage resultImage = SKImage.FromBitmap(result);
        using SKData data = resultImage.Encode(SKEncodedImageFormat.Png, 100);
        using FileStream stream = File.OpenWrite(outputPath);

        data.SaveTo(stream);

        return dimensions;
    }
    
    private static string GetOutputPath(string inputPath, string outputDirPath)
    {
        var filename = Path.GetFileName(inputPath);
        return Path.Combine(outputDirPath, filename);
    }
}
