﻿using Deface.NET.Configuration.Provider.Interfaces;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class VideoReader
(
    IScopedSettingsProvider settingsProvider,
    IVideoInfoProvider videoInfoProvider,
    IExternalProcessFactory externalProcessFactory
) : IVideoReader
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly IVideoInfoProvider _videoInfoProvider = videoInfoProvider;
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    public VideoReadResult ReadVideo(string videoFilePath)
    {
        var videoInfo = _videoInfoProvider.GetInfo(videoFilePath);

        using var process = GetFfmpegProcess(videoInfo);
        process.Start();

        VideoStreamProcessor processor = new(videoInfo);
        var frames = processor.ReadAllFrames(process.OutputStream);

        return new(frames, videoInfo);
    }

    private IExternalProcess GetFfmpegProcess(VideoInfo videoInfo)
    {
        var args = string.Join(" ",
        [
            "-loglevel", "panic",
            "-i", $"\"{videoInfo.Path}\"",
            "-f", "image2pipe",
            "-pix_fmt", "bgra",
            "-vcodec", "rawvideo",
            "-"
        ]);

        return _externalProcessFactory.CreateExternalProcess(_settings.FFMpegPath, args);
    }
}
