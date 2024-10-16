using Deface.NET.CenterFace;
using Deface.NET.Logging;
using OpenCvSharp;
using System.Diagnostics;

namespace Deface.NET.Processing;

internal sealed class ImageProcessor(Settings settings, DLogger<IDefaceService> logger) : ProcessorBase(settings), IDisposable
{
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly CenterFaceModel _centerFace = new();

    public void Dispose() => _centerFace.Dispose();

    public ProcessingResult Process(string inputPath, string outputPath, Action<Settings>? customSettings)
    {
        ApplyScopedSettings(customSettings);

        Stopwatch stopwatch = new();
        stopwatch.Start();

        Mat image = new(inputPath);
        Size imageSize = image.Size();

        var detectedObjects = _centerFace.Detect(image, imageSize, Settings.Threshold);
        ShapeDrawer.DrawShapes(image, detectedObjects, Settings);

        Cv2.ImWrite(outputPath, image, [(int)ImwriteFlags.PngCompression, 3]);

        stopwatch.Stop();

        _logger.Log(DefaceLoggingLevel.Basic, "Processed {Input} and saved to {Output}", inputPath, outputPath);

        return new(inputPath, outputPath, stopwatch.Elapsed, imageSize, imageSize, Settings.Threshold, 1);
    }
}
