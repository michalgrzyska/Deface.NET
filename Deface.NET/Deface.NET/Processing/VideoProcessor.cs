using Deface.NET.Common;
using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing.Models;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.Processing;

internal sealed class VideoProcessor
(
    IScopedSettingsProvider settingsProvider,
    IDLogger<IDefaceService> logger,
    IObjectDetector detector,
    IVideoWriter videoWriter,
    IVideoReader videoReader,
    IShapeDrawerProvider shapeDrawerProvider,
    IFrameCreator frameCreator
) : IVideoProcessor
{
    private readonly IDLogger<IDefaceService> _logger = logger;
    private readonly IObjectDetector _detector = detector;
    private readonly IVideoWriter _videoWriter = videoWriter;
    private readonly IVideoReader _videoReader = videoReader;
    private readonly IShapeDrawerProvider _shapeDrawerProvider = shapeDrawerProvider;
    private readonly IFrameCreator _frameCreator = frameCreator;
    private readonly Settings _settings = settingsProvider.Settings;

    private List<DetectedObject> _lastDetectedObjects = [];

    public ProcessingResult Process(string inputPath, string outputPath)
    {
        LogProcessingStarted(inputPath);
        var processedFrames = GetProcessedFrames(inputPath);

        LogSavingVideo(inputPath);
        _videoWriter.WriteVideo(processedFrames.Frames, processedFrames.VideoInfo, outputPath);

        LogVideoProcessed(inputPath, processedFrames, outputPath);
        return GetProcessingResult(inputPath, outputPath, processedFrames);
    }

    private ProcessedFrames GetProcessedFrames(string inputPath)
    {
        var progressLogger = _logger.GetProgressLogger();
        progressLogger.Start();

        List<Frame> processedFrames = [];

        var videoInfo = _videoReader.ReadVideo((frameInfo) =>
        {
            var processedFrame = ProcessFrame(frameInfo);
            processedFrames.Add(processedFrame);

            progressLogger.Log(frameInfo.Index + 1, "Processing video frames", frameInfo.TotalFrames);
        }, inputPath);

        var processingTime = progressLogger.Stop();
        return new(processedFrames, videoInfo, processingTime);
    }

    private Frame ProcessFrame(FrameInfo frameInfo)
    {
        var frame = _frameCreator.FromBgrArray(frameInfo.BgrData, frameInfo.Width, frameInfo.Height);

        if (frameInfo.Index % _settings.RunDetectionEachNFrames == 0)
        {
            _lastDetectedObjects = _detector.Detect(frame, _settings);
        }

        return _shapeDrawerProvider.ShapeDrawer.Draw(frame, _lastDetectedObjects);
    }

    private ProcessingResult GetProcessingResult(string inputPath, string outputPath, ProcessedFrames processedFrames)
    {
        return new(inputPath, outputPath, processedFrames.ProcessingTime, _settings.Threshold, processedFrames.VideoInfo.AverageFps);
    }

    private void LogProcessingStarted(string inputPath) => _logger.LogBasic("Video processing started for \"{InputPath}\"", inputPath);

    private void LogSavingVideo(string inputPath) => _logger.LogDetailed("Saving processed video \"{InputPath}\" to a destinate location", inputPath);

    private void LogVideoProcessed(string inputPath, ProcessedFrames processedFrames, string outputPath) =>
        _logger.LogBasic("Video \"{InputPath}\" processed in {Time} and saved to \"{OutputPath}\"", inputPath, processedFrames.ProcessingTime, outputPath);
}
