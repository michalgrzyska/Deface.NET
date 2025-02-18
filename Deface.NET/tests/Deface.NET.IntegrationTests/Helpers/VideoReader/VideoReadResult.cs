using SkiaSharp;

namespace Deface.NET.IntegrationTests.Helpers.VideoReader;

internal record VideoReadResult(List<SKBitmap> Frames, VideoInfoStreamOutput VideoInfo);
