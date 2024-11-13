using Deface.NET.Graphics;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics;

internal static class TestFrameHelper
{
    private const int Width = 1000;
    private const int Height = 1000;

    public static Frame GetTestFrame()
    {
        using MemoryStream stream = GetFrameStream();
        return new(stream);
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
