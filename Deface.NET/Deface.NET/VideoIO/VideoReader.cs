﻿using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Models;
using Deface.NET.VideoIO.Interfaces;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO;

internal class VideoReader(IScopedSettingsProvider settingsProvider, IVideoInfoProvider videoInfoService) : IVideoReader
{
    private readonly Settings _settings = settingsProvider.Settings;
    private readonly IVideoInfoProvider _videoInfoService = videoInfoService;

    public async Task<VideoInfo> ReadVideo(Func<Frame, int, int, Task> frameProcess, string videoFilePath)
    {
        VideoInfo videoInfo = await _videoInfoService.GetInfo(videoFilePath);

        using VideoFileSystemReader videoReader = new(videoFilePath, frameProcess, _settings.FFMpegPath, videoInfo);
        await videoReader.ProcessVideo();

        return videoInfo;
    }
}
