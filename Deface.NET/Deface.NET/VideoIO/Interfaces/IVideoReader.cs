using Deface.NET.Graphics.Models;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Interfaces;

internal interface IVideoReader
{
    Task<VideoInfo> ReadVideo(Func<FrameInfo, Task> frameProcess, string videoFilePath);
}