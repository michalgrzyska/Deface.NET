﻿using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Models;
using Deface.NET.System.ExternalProcessing;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;
using System.Globalization;

namespace Deface.NET.VideoIO;

internal class VideoWriter(IScopedSettingsProvider settingsProvider, IExternalProcessFactory externalProcessFactory) : IVideoWriter
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly IExternalProcessFactory _externalProcessFactory = externalProcessFactory;

    public void WriteVideo(List<Frame> frames, VideoInfo videoInfo, string outputPath)
    {
        using var ffmpegProcess = GetFfmpegProcess(videoInfo, outputPath);
        ffmpegProcess.Start();

        using var ffmpegInput = ffmpegProcess.InputStream;

        foreach (var frame in frames)
        {
            ffmpegInput.Write(frame.Bytes);
        }
    }

    private IExternalProcess GetFfmpegProcess(VideoInfo videoInfo, string outputPath)
    {
        var ffmpegPath = _settings.FFMpegPath;

        var args = string.Join(" ",
        [
            "-loglevel", "panic",
            "-y",
            "-f", "rawvideo",
            "-pixel_format", "bgra",
            "-video_size", $"{videoInfo.Width}x{videoInfo.Height}",
            "-framerate",  videoInfo.AverageFps.ToString(CultureInfo.InvariantCulture),
            "-i", "-",
            "-c:v", "libx264",
            "-pix_fmt yuv420p",
            $"\"{outputPath}\""
        ]);

        return _externalProcessFactory.CreateExternalProcess(ffmpegPath, args, redirectStandardInput: true);
    }
}
