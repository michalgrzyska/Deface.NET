using Deface.NET.Graphics;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.ObjectDetection.UltraFace;
using Deface.NET.VideoIO;
using SkiaSharp;

namespace Deface.NET.Processing;

internal sealed class VideoProcessor(Settings settings, DLogger<IDefaceService> logger) : ProcessorBase(settings), IDisposable
{
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly UltraFaceDetector _ultraFaceDetector = new();

    private List<DetectedObject> _lastDetectedObjects = [];

    public void Dispose() { }

    public async Task<ProcessingResult> Process(string inputPath, string outputPath, Action<Settings>? customSettings)
    {
        ApplyScopedSettings(customSettings);

        var (videoInfo, processedFrames, time) = await GetProcessedFrames(inputPath);

        SaveVideo(processedFrames, videoInfo, outputPath);

        _logger.Log(DefaceLoggingLevel.Basic, "Video {InputPath} processed in {Time} and saved to {OutputPath}", inputPath, time, outputPath);

        return new ProcessingResult(inputPath, outputPath, time, Settings.Threshold, videoInfo.AverageFps);
    }

    private async Task<(VideoInfo, List<SKBitmap>, TimeSpan)> GetProcessedFrames(string inputPath)
    {
        var progressLogger = _logger.GetProgressLogger();
        progressLogger.Start();

        List<SKBitmap> processedFrames = [];

        using VideoReader videoReader = new(inputPath, Settings, (frame, index, totalFrames) =>
        {
            SKBitmap processedFrame = ProcessFrame(frame, index);
            processedFrames.Add(processedFrame);

            progressLogger.LogProgress(index + 1, "Processing video frames", totalFrames);
            return Task.CompletedTask;
        });

        VideoInfo videoInfo = await videoReader.Start();
        TimeSpan processingTime = progressLogger.Stop();

        return (videoInfo, processedFrames, processingTime);
    }

    private SKBitmap ProcessFrame(SKBitmap frame, int i)
    {
        if (i % Settings.RunDetectionEachNFrames == 0)
        {
            _lastDetectedObjects = _ultraFaceDetector.Detect(frame);
        }

        SKBitmap processedFrame = ShapeDrawer.DrawShapes(frame, _lastDetectedObjects, Settings);
        return processedFrame;
    }

    private void SaveVideo(List<SKBitmap> processedFrames, VideoInfo videoInfo, string outputPath)
    {
        var framesAsBytesArray = processedFrames
            .Select(GraphicsHelper.ConvertSKBitmapToRgbByteArray)
            .ToList();

        VideoWriter.WriteVideo(framesAsBytesArray, videoInfo, outputPath, Settings);
    }
}
