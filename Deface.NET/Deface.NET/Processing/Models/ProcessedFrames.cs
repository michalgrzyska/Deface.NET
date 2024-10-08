using Deface.NET.Graphics.Models;
using Deface.NET.VideoIO.Models;

namespace Deface.NET.Processing.Models;

internal record ProcessedFrames(List<Frame> Frames, VideoInfo VideoInfo, TimeSpan ProcessingTime);