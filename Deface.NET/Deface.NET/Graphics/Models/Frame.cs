using Deface.NET.Graphics.Interfaces;
using SkiaSharp;

namespace Deface.NET.Graphics.Models;

internal sealed class Frame : IDisposable, ISize
{
    private readonly SKBitmap _bitmap;

    public int Width => _bitmap.Width;
    public int Height => _bitmap.Height;
    public byte[] Bytes => _bitmap.Bytes;

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
        using var resultImage = SKImage.FromBitmap(_bitmap);
        using var data = resultImage.Encode(imageFormat.Format, imageFormat.Quality);

        return data.ToArray();
    }

    public Frame AsRescaledWithPadding(int targetWidth, int targetHeight)
    {
        var scale = Math.Min((float)targetWidth / Width, (float)targetHeight / Height);

        var scaledWidth = (int)(Width * scale);
        var scaledHeight = (int)(Height * scale);

        var offsetX = (targetWidth - scaledWidth) / 2;
        var offsetY = (targetHeight - scaledHeight) / 2;

        SKBitmap outputBitmap = new(targetWidth, targetHeight);

        using SKCanvas canvas = new(outputBitmap);

        canvas.Clear(SKColors.Black);
        canvas.DrawBitmap(_bitmap, new SKRect(offsetX, offsetY, offsetX + scaledWidth, offsetY + scaledHeight));

        return new(outputBitmap);
    }

    public static explicit operator SKBitmap(Frame frame) => frame._bitmap;
    public static explicit operator Frame(SKBitmap bitmap) => new(bitmap);
}
