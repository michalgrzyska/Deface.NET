using Deface.NET.Graphics.Models;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.VideoIO.Interfaces;

internal interface IVideoWriter
{
    void WriteVideo(List<Frame> frames, VideoInfo videoInfo, string outputPath);
}