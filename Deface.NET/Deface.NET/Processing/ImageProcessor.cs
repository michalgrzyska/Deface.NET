using Deface.NET.CenterFace;
using Deface.NET.Logging;
using OpenCvSharp;
using System.Diagnostics;

namespace Deface.NET.Processing;

internal sealed class ImageProcessor
(
    Settings settings,
    ShapeDrawingService shapeDrawingService,
    DLogger<IDefaceService> logger
) : IDisposable
{
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly ShapeDrawingService _shapeDrawingService = shapeDrawingService;
    private readonly Settings _settings = settings;

    private readonly CenterFaceModel _centerFace = new();

    public void Dispose() => _centerFace.Dispose();

    public ProcessingResult Process(string inputPath, string outputPath)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        Mat image = new(inputPath);
        Size imageSize = image.Size();

        var detectedObjects = _centerFace.Detect(image, imageSize, _settings.Threshold);
        _shapeDrawingService.DrawShapes(image, detectedObjects);

        Cv2.ImWrite(outputPath, image, [(int)ImwriteFlags.PngCompression, 3]);

        stopwatch.Stop();

        _logger.Log(DefaceLoggingLevel.Basic, "Processed {Input} and saved to {Output}", inputPath, outputPath);

        return new(inputPath, outputPath, stopwatch.Elapsed, imageSize, imageSize, _settings.Threshold, 1);
    }
}
