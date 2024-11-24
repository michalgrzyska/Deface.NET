using Deface.NET.Graphics.Models;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics.Helpers;

internal static class TestFrameHelper
{
    private const int Width = 1000;
    private const int Height = 1000;

    public static Frame GetTestFrame(string path)
    {
        using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
        return new(fileStream);
    }

    public static Frame GetTestFrame()
    {
        using MemoryStream stream = GetFrameStream();
        return new(stream);
    }

    public static Frame GetTestFrameWithMesh()
    {
        Frame frame = GetTestFrame();

        var nativeElement = (SKBitmap)frame;

        for (var y = 0; y < nativeElement.Height; y++)
        {
            for (var x = 0; x < nativeElement.Width; x++)
            {
                SKColor color = x % 2 == y % 2
                    ? new SKColor(0, 0, 0)
                    : new SKColor(255, 255, 255);

                nativeElement.SetPixel(x, y, color);
            }
        }

        return frame;
    }

    private static MemoryStream GetFrameStream()
    {
        SKBitmap bitmap = new(Width, Height);

        using (var canvas = new SKCanvas(bitmap))
        {
            canvas.Clear(SKColors.White);
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        var stream = new MemoryStream();
        data.SaveTo(stream);

        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }
}
