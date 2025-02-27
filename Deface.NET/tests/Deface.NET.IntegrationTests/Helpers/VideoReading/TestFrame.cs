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

    public void HasRectangle(int x1, int y1, int x2, int y2, SKColor color)
    {
        int maxX = Math.Max(x1, x2);
        int minX = Math.Min(x1, x2);
        int maxY = Math.Max(y1, y2);
        int minY = Math.Min(y1, y2);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var pixel = GetPixel(x, y);
                pixel.ShouldBe(color);
            }
        }
    }
}
