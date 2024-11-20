﻿using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Logging;
using Deface.NET.ObjectDetection;
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
    IShapeDrawer shapeDrawer
) : IDisposable
{
    private readonly IDLogger<IDefaceService> _logger = logger;
    private readonly IObjectDetector _detector = detector;
    private readonly IVideoWriter _videoWriter = videoWriter;
    private readonly IVideoReader _videoReader = videoReader;
    private readonly IShapeDrawer _shapeDrawer = shapeDrawer;

    private readonly Settings _settings = settingsProvider.Settings;

    private List<DetectedObject> _lastDetectedObjects = [];

    public void Dispose() => _detector.Dispose();

    public async Task<ProcessingResult> Process(string inputPath, string outputPath)
    {
        _logger.Log(DefaceLoggingLevel.Basic, "Video processing started for \"{InputPath}\"", inputPath);

        var (videoInfo, processedFrames, time) = await GetProcessedFrames(inputPath);

        _logger.Log(DefaceLoggingLevel.Detailed, "Saving processed video \"{InputPath}\" to a destinate location", inputPath);

        _videoWriter.WriteVideo(processedFrames, videoInfo, outputPath);

        _logger.Log(DefaceLoggingLevel.Basic, "Video \"{InputPath}\" processed in {Time} and saved to \"{OutputPath}\"", inputPath, time, outputPath);

        return new ProcessingResult(inputPath, outputPath, time, _settings.Threshold, videoInfo.AverageFps);
    }

    private async Task<(VideoInfo, List<Frame>, TimeSpan)> GetProcessedFrames(string inputPath)
    {
        var progressLogger = _logger.GetProgressLogger();
        progressLogger.Start();

        List<Frame> processedFrames = [];

        VideoInfo videoInfo = await _videoReader.ReadVideo((frame, index, totalFrames) =>
        {
            Frame processedFrame = ProcessFrame(frame, index);
            processedFrames.Add(processedFrame);

            progressLogger.LogProgress(index + 1, "Processing video frames", totalFrames);
            return Task.CompletedTask;
        }, inputPath);

        TimeSpan processingTime = progressLogger.Stop();

        return (videoInfo, processedFrames, processingTime);
    }

    private Frame ProcessFrame(Frame frame, int i)
    {
        if (i % _settings.RunDetectionEachNFrames == 0)
        {
            _lastDetectedObjects = _detector.Detect(frame, _settings);
        }

        Frame processedFrame = _shapeDrawer.DrawShapes(frame, _lastDetectedObjects);
        return processedFrame;
    }
}
