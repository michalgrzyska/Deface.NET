using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Interfaces;

internal interface IVideoReader
{
    VideoInfo ReadVideo(Action<FrameInfo> frameProcess, string videoFilePath);
}