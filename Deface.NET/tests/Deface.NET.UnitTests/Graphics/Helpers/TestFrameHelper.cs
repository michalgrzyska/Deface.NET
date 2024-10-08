using Deface.NET.Graphics.Models;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics.Helpers;

internal static class TestFrameHelper
{
    private const int Width = 1000;
    private const int Height = 1000;

    private static readonly SKColor BlackPixel = new(0, 0, 0);
    private static readonly SKColor WhitePixel = new(255, 255, 255);

    public static Frame GetTestFrame(string path)
    {
        var imageBytes = File.ReadAllBytes(path);

        using (var stream = new MemoryStream(imageBytes))
        using (var codec = SKCodec.Create(stream))
        {
            // Create an SKImageInfo with the desired color type
            var imageInfo = new SKImageInfo(codec.Info.Width, codec.Info.Height, SKColorType.Bgra8888);

            // Create an empty SKBitmap with the desired color type
            var bitmap = new SKBitmap(imageInfo);

            // Decode the image data into the bitmap
            var result = codec.GetPixels(imageInfo, bitmap.GetPixels());

            return (Frame)bitmap;
        }
    }

    public static Frame GetTestFrame()
    {
        using var stream = GetFrameStream();
        return (Frame)SKBitmap.Decode(stream);
    }

    public static Frame GetTestFrameWithMesh()
    {
        var frame = GetTestFrame();
        var nativeElement = (SKBitmap)frame;
        var pixels = new SKColor[nativeElement.Width * nativeElement.Height];
        var isBlack = true;

        for (int y = 0; y < nativeElement.Height; y++)
        {
            for (int x = 0; x < nativeElement.Width; x++)
            {
                pixels[y * nativeElement.Width + x] = isBlack ? BlackPixel : WhitePixel;
                isBlack = !isBlack;
            }

            isBlack = nativeElement.Width % 2 == 0 ? isBlack : !isBlack;
        }

        nativeElement.Pixels = pixels;

        return frame;
    }

    private static MemoryStream GetFrameStream()
    {
        SKBitmap bitmap = new(Width, Height);

        using (SKCanvas canvas = new(bitmap))
        {
            canvas.Clear(SKColors.White);
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        MemoryStream stream = new();
        data.SaveTo(stream);

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
}
