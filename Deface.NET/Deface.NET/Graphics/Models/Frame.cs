using SkiaSharp;
using System.Runtime.InteropServices;

namespace Deface.NET.Graphics.Models;

internal sealed class Frame : IDisposable
{
    private readonly SKBitmap _bitmap;

    public int Width => _bitmap.Width;
    public int Height => _bitmap.Height;

    private Frame(SKBitmap bitmap)
    {
        _bitmap = bitmap;
    }

    public void Dispose() => _bitmap.Dispose();

    public Pixel GetPixel(int x, int y)
    {
        var pixel = _bitmap.GetPixel(x, y);
        return new(pixel.Red, pixel.Green, pixel.Blue);
    }

    public byte[] ToByteArray(ImageFormat imageFormat)
    {
        using SKImage resultImage = SKImage.FromBitmap(_bitmap);
        using SKData data = resultImage.Encode(imageFormat.Format, imageFormat.Quality);

        return data.ToArray();
    }

    public byte[] ToByteArray()
    {
        int width = _bitmap.Width;
        int height = _bitmap.Height;
        int bytesPerPixel = 3;
        byte[] rgbData = new byte[width * height * bytesPerPixel];

        using SKImage image = SKImage.FromBitmap(_bitmap);
        using SKPixmap pixmap = image.PeekPixels();

        byte[] bgraData = new byte[width * height * 4];
        var handle = GCHandle.Alloc(bgraData, GCHandleType.Pinned);

        try
        {
            nint bgraDataPtr = handle.AddrOfPinnedObject();
            pixmap.ReadPixels(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul), bgraDataPtr, width * 4);

            for (int i = 0, j = 0; i < bgraData.Length; i += 4, j += 3)
            {
                rgbData[j] = bgraData[i + 2];
                rgbData[j + 1] = bgraData[i + 1];
                rgbData[j + 2] = bgraData[i];
            }
        }
        finally
        {
            handle.Free();
        }

        return rgbData;
    }

    public Frame AsRescaledWithPadding(int targetWidth, int targetHeight)
    {
        float scale = Math.Min((float)targetWidth / Width, (float)targetHeight / Height);

        int scaledWidth = (int)(Width * scale);
        int scaledHeight = (int)(Height * scale);

        int offsetX = (targetWidth - scaledWidth) / 2;
        int offsetY = (targetHeight - scaledHeight) / 2;

        SKBitmap outputBitmap = new(targetWidth, targetHeight);

        using SKCanvas canvas = new(outputBitmap);

        canvas.Clear(SKColors.Black);
        canvas.DrawBitmap(_bitmap, new SKRect(offsetX, offsetY, offsetX + scaledWidth, offsetY + scaledHeight));

        return new(outputBitmap);
    }

    public static explicit operator SKBitmap(Frame frame) => frame._bitmap;
    public static explicit operator Frame(SKBitmap bitmap) => new(bitmap);
}
