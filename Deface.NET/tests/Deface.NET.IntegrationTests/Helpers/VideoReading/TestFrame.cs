using SkiaSharp;
using System.Runtime.InteropServices;

namespace Deface.NET.IntegrationTests.Helpers.VideoReading;

internal class TestFrame : SKBitmap
{
    public TestFrame(byte[] bgraData, int width, int height) : base(width, height, SKColorType.Bgra8888, SKAlphaType.Premul)
    {
        var pixelPtr = GetPixels();
        Marshal.Copy(bgraData, 0, pixelPtr, bgraData.Length);
    }
}
