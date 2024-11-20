using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Interfaces;

internal interface IVideoInfoProvider
{
    Task<VideoInfo> GetInfo(string filePath);
}