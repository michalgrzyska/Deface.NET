using Deface.NET.Graphics.Models;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Interfaces;

internal interface IVideoReader
{
    Task<VideoInfo> ReadVideo(Func<Frame, int, int, Task> frameProcess, string videoFilePath);
}