using SkiaSharp;
using System.Runtime.InteropServices;

namespace Deface.NET.Graphics.Models;

internal class Frame : IDisposable
{
    private SKBitmap _bitmap;

    public int Width => _bitmap.Width;
    public int Height => _bitmap.Height;

    public Frame(byte[] bgrData, int width, int height)
    {
        if (width * height * 3 != bgrData.Length)
        {
            throw new ArgumentException($"{nameof(bgrData)} length must be the size of width * height * 3");
        }

        byte[] rgbaData = GraphicsHelper.ConvertBgrToRgba(bgrData, width, height);
        _bitmap = GraphicsHelper.GetBgraBitmapFromBytes(rgbaData, width, height);
    }

    public Frame(Stream stream)
    {
        _bitmap = SKBitmap.Decode(stream);
    }

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

    public SKBitmap GetNativeElement() => _bitmap;

    public void UpdateNativeElement(SKBitmap bitmap)
    {
        _bitmap.Dispose();
        _bitmap = bitmap;
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
}
