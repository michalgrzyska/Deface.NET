using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Interfaces;

internal interface IVideoInfoProvider
{
    VideoInfo GetInfo(string filePath);
}