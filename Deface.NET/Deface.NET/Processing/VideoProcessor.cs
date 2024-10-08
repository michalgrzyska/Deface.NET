using Deface.NET.CenterFace;
using Deface.NET.Logging;
using OpenCvSharp;

namespace Deface.NET.Processing;

internal sealed class VideoProcessor
(
    Settings settings,
    DLogger<IDefaceService> logger,
    ShapeDrawingService shapeDrawingService
) : IDisposable
{
    private readonly Settings _settings = settings;
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly ShapeDrawingService _shapeDrawingService = shapeDrawingService;

    private readonly CenterFaceModel _centerFace = new();

    private List<FaceInfo> _lastDetectedObjects = [];

    public void Dispose() => _centerFace.Dispose();

    public ProcessingResult Process(string inputPath, string outputPath)
    {
        using VideoCapture capture = new(inputPath);
        Size captureSize = capture.ToSize();

        using VideoWriter writer = new(outputPath, FourCC.FromString("mp4v"), capture.Fps, captureSize);

        using Mat frame = new();
        Size adjustedSize = GetAdjustedSize(capture);

        var progressLogger = _logger.GetProgressLogger(capture.FrameCount);
        progressLogger.Start();

        for (int i = 0; i < capture.FrameCount; i++)
        {
            capture.Read(frame);

            TryUpdateLastDetectedObjects(frame, adjustedSize, i);
            _shapeDrawingService.DrawShapes(frame, _lastDetectedObjects);

            writer.Write(frame);

            progressLogger.LogProgress(i + 1, "Processing video frames");
        }

        TimeSpan processingTime = progressLogger.Stop();
        _logger.Log(DefaceLoggingLevel.Basic, "Video processing result:\n\tFile: \"{Path}\"\n\tTime: {Time}", inputPath, processingTime.ToString(@"hh\:mm\:ss\:fff"));

        return new(inputPath, outputPath, processingTime, captureSize, adjustedSize, _settings.Threshold, capture.Fps);
    }

    private Size GetAdjustedSize(VideoCapture videoCapture)
    {
        if (!_settings.RescaleVideoWithShorterSideEqualsTo.HasValue)
        {
            return new(videoCapture.FrameWidth, videoCapture.FrameHeight);
        }

        return ResolutionRescaler.RescaleIfNeeded(videoCapture, _settings.RescaleVideoWithShorterSideEqualsTo.Value);
    }

    private void TryUpdateLastDetectedObjects(Mat frame, Size size, int i)
    {
        if (i % _settings.RunDetectionEachNFrames == 0)
        {
            _lastDetectedObjects = _centerFace.Detect(frame, size, scoreThreshold: _settings.Threshold);
        }
    }
}
