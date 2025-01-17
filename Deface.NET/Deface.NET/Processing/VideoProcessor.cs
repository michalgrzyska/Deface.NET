using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
using Deface.NET.Processing.Models;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.Processing;

internal sealed class VideoProcessor
: IVideoProcessor
{
    private readonly IDLogger<IDefaceService> _logger;
    private readonly IObjectDetector _detector;
    private readonly IVideoWriter _videoWriter;
    private readonly IVideoReader _videoReader;
    private readonly IShapeDrawerProvider _shapeDrawerProvider;
    private readonly IFrameCreator _frameCreator;
    private readonly Settings _settings;

    private List<DetectedObject> _lastDetectedObjects = [];

    public VideoProcessor(
        IScopedSettingsProvider settingsProvider,
        IDLogger<IDefaceService> logger,
        IObjectDetector detector,
        IVideoWriter videoWriter,
        IVideoReader videoReader,
        IShapeDrawerProvider shapeDrawerProvider,
        IFrameCreator frameCreator,
        IVideoEncoderChecker videoEncoderChecker)
    {
        _logger = logger;
        _detector = detector;
        _videoWriter = videoWriter;
        _videoReader = videoReader;
        _shapeDrawerProvider = shapeDrawerProvider;
        _frameCreator = frameCreator;
        _settings = settingsProvider.Settings;

        videoEncoderChecker.CheckFfmpegCodecs();
    }

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

        var videoReadResult = _videoReader.ReadVideo(inputPath);
        List<Frame> processedFrames = [];

        for (var i = 0; i < videoReadResult.Frames.Count; i++)
        {
            var processedFrame = ProcessFrame(videoReadResult.Frames[i], videoReadResult.VideoInfo, i);
            processedFrames.Add(processedFrame);

            progressLogger.Log(i + 1, "Processing video frames", videoReadResult.Frames.Count);
        }

        var processingTime = progressLogger.Stop();
        return new(processedFrames, videoReadResult.VideoInfo, processingTime);
    }

    private Frame ProcessFrame(byte[] frameBytes, VideoInfo videoInfo, int index)
    {
        var frame = _frameCreator.FromBgraArray(frameBytes, videoInfo.Width, videoInfo.Height);

        if (index % _settings.RunDetectionEachNFrames == 0)
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
