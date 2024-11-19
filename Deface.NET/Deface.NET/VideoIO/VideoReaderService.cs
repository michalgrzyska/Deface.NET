using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Models;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class VideoReaderService(IScopedSettingsProvider settingsProvider, VideoInfoService videoInfoService)
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly VideoInfoService _videoInfoService = videoInfoService;

    public async Task<VideoInfo> ReadVideo(Func<Frame, int, int, Task> frameProcess, string videoFilePath)
    {
        VideoInfo videoInfo = await _videoInfoService.GetInfo(videoFilePath);

        using VideoReader videoReader = new(videoFilePath, frameProcess, _settings.FFMpegPath, videoInfo);
        await videoReader.ProcessVideo();

        return videoInfo;
    }
}
