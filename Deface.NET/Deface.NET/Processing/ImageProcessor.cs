using Deface.NET.CenterFace;
using Deface.NET.Logging;
using OpenCvSharp;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Deface.NET.Processing;

internal sealed class ImageProcessor(Settings settings, DLogger<IDefaceService> logger) : ProcessorBase(settings), IDisposable
{
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly CenterFaceModel _centerFace = new();

    private readonly static string[] ImageExtensions = [".jpg", ".jpeg", ".png"];

    public void Dispose() => _centerFace.Dispose();

    public ProcessingResult Process(string inputPath, string outputPath, Action<Settings>? customSettings)
    {
        ApplyScopedSettings(customSettings);

        Stopwatch stopwatch = new();
        stopwatch.Start();

        using Mat image = LoadImage(inputPath);
        Size size = ProcessImage(image, outputPath);

        stopwatch.Stop();

        _logger.Log(DefaceLoggingLevel.Basic, "Processed {Input} and saved to {Output}", inputPath, outputPath);
        return new(inputPath, outputPath, stopwatch.Elapsed, size, size, Settings.Threshold, 1);
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

    private static Mat LoadImage(string path)
    {
        try
        {
            Mat image = new(path);

            if (image is null || image.Empty())
            {
                throw new DefaceException($"Could not open image {path}.");
            }

            return image;
        }
        catch (Exception e)
        {
            throw new DefaceException($"Could not open image {path}.", e);
        }
    }

    private Size ProcessImage(Mat image, string outputPath)
    {
        Size imageSize = image.Size();

        var detectedObjects = _centerFace.Detect(image, imageSize, Settings.Threshold);
        ShapeDrawer.DrawShapes(image, detectedObjects, Settings);

        Cv2.ImWrite(outputPath, image, [(int)ImwriteFlags.PngCompression, 3]);
        return imageSize;
    }
    
    private static string GetOutputPath(string inputPath, string outputDirPath)
    {
        var filename = Path.GetFileName(inputPath);
        return Path.Combine(outputDirPath, filename);
    }
}
