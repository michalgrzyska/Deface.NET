using Deface.NET.Configuration.Provider;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class VideoReader(IScopedSettingsProvider settingsProvider, IVideoInfoProvider videoInfoService) : IVideoReader
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly IVideoInfoProvider _videoInfoService = videoInfoService;

    public async Task<VideoInfo> ReadVideo(Action<FrameInfo> frameProcess, string videoFilePath)
    {
        VideoInfo videoInfo = await _videoInfoService.GetInfo(videoFilePath);

        using FFMpegVideoReader videoReader = new(videoFilePath, frameProcess, _settings.FFMpegPath, videoInfo);
        videoReader.ProcessVideo();

        return videoInfo;
    }
}
