namespace Deface.NET.VideoIO.Models;

internal record FrameInfo(byte[] BgrData, int Index, int TotalFrames, int Width, int Height);