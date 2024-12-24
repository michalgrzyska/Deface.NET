using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Interfaces;

internal interface IVideoReader
{
    VideoReadResult ReadVideo(string videoFilePath);
}