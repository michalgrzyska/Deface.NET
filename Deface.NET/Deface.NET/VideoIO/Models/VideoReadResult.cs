namespace Deface.NET.VideoIO.Models;

internal record VideoReadResult(List<byte[]> Frames, VideoInfo VideoInfo);