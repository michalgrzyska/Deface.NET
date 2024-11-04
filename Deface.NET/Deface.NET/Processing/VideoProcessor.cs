using Deface.NET.Graphics;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.VideoIO;

namespace Deface.NET.Processing;

internal sealed class VideoProcessor(Settings settings, DLogger<IDefaceService> logger, ObjectDetector detector) 
    : ProcessorBase(settings), IDisposable
{
    private readonly DLogger<IDefaceService> _logger = logger;
    private readonly ObjectDetector _detector = detector;

    private List<DetectedObject> _lastDetectedObjects = [];

    public void Dispose() => _detector.Dispose();

    public async Task<ProcessingResult> Process(string inputPath, string outputPath, Action<Settings>? customSettings)
    {
        ApplyScopedSettings(customSettings);

        _logger.Log(DefaceLoggingLevel.Basic, "Video processing started for \"{InputPath}\"", inputPath);

        var (videoInfo, processedFrames, time) = await GetProcessedFrames(inputPath);

        _logger.Log(DefaceLoggingLevel.Detailed, "Saving processed video \"{InputPath}\" to a destinate location", inputPath);

        VideoWriter.WriteVideo(processedFrames, videoInfo, outputPath, Settings);

        _logger.Log(DefaceLoggingLevel.Basic, "Video \"{InputPath}\" processed in {Time} and saved to \"{OutputPath}\"", inputPath, time, outputPath);

        return new ProcessingResult(inputPath, outputPath, time, Settings.Threshold, videoInfo.AverageFps);
    }

    private async Task<(VideoInfo, List<Frame>, TimeSpan)> GetProcessedFrames(string inputPath)
    {
        var progressLogger = _logger.GetProgressLogger();
        progressLogger.Start();

        List<Frame> processedFrames = [];

        using VideoReader videoReader = new(inputPath, Settings, (frame, index, totalFrames) =>
        {
            Frame processedFrame = ProcessFrame(frame, index);
            processedFrames.Add(processedFrame);

            progressLogger.LogProgress(index + 1, "Processing video frames", totalFrames);
            return Task.CompletedTask;
        });

        VideoInfo videoInfo = await videoReader.Start();
        TimeSpan processingTime = progressLogger.Stop();

        return (videoInfo, processedFrames, processingTime);
    }

    private Frame ProcessFrame(Frame frame, int i)
    {
        if (i % Settings.RunDetectionEachNFrames == 0)
        {
            _lastDetectedObjects = _detector.Detect(frame, Settings);
        }

        Frame processedFrame = ShapeDrawer.DrawShapes(frame, _lastDetectedObjects, Settings);
        return processedFrame;
    }
}
