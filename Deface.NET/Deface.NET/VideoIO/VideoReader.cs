using Deface.NET.Configuration.Provider;
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

    public VideoInfo ReadVideo(Action<FrameInfo> frameProcess, string videoFilePath)
    {
        var videoInfo = _videoInfoProvider.GetInfo(videoFilePath);

        using var process = GetFfmpegProcess(videoInfo);
        process.Start();

        VideoStreamProcessor processor = new(videoInfo);
        processor.Process(process.OutputStream, frameProcess);

        return videoInfo;
    }

    private IExternalProcess GetFfmpegProcess(VideoInfo videoInfo)
    {
        string args = string.Join(" ",
        [
            "-loglevel", "panic",
            "-i", $"\"{videoInfo.Path}\"",
            "-f", "image2pipe",
            "-pix_fmt", "rgb24",
            "-vcodec", "rawvideo",
            "-"
        ]);

        return _externalProcessFactory.CreateExternalProcess(_settings.FFMpegPath, args);
    }
}
