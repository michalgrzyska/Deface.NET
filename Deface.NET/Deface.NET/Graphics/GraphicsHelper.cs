using SkiaSharp;
using System.Runtime.InteropServices;

namespace Deface.NET.Graphics;

internal static class GraphicsHelper
{
    public static byte[] ConvertBgrToRgba(byte[] bgr, int width, int height)
    {
        byte[] rgbaData = new byte[width * height * 4];

        for (int j = 0; j < width * height; j++)
        {
            int b = bgr[j * 3];
            int g = bgr[j * 3 + 1];
            int r = bgr[j * 3 + 2];

            rgbaData[j * 4] = (byte)r;
            rgbaData[j * 4 + 1] = (byte)g;
            rgbaData[j * 4 + 2] = (byte)b;
            rgbaData[j * 4 + 3] = 255;
        }

        return rgbaData;
    }

    public static SKBitmap GetBitmapFromBytes(byte[] bytes, int width, int height)
    {
        SKBitmap bitmap = new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        nint pixelPointer = handle.AddrOfPinnedObject();

        bitmap.InstallPixels(new SKImageInfo(width, height, SKColorType.Bgra8888), pixelPointer, width * 4);
        return bitmap;
    }

    public static byte[] ConvertSKBitmapToRgbByteArray(SKBitmap bitmap)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        int bytesPerPixel = 3;
        byte[] rgbData = new byte[width * height * bytesPerPixel];

        using SKImage image = SKImage.FromBitmap(bitmap);
        using SKPixmap pixmap = image.PeekPixels();

        byte[] bgraData = new byte[width * height * 4];
        var handle = GCHandle.Alloc(bgraData, GCHandleType.Pinned);

        try
        {
            IntPtr bgraDataPtr = handle.AddrOfPinnedObject();

            if (!pixmap.ReadPixels(new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul), bgraDataPtr, width * 4))
            {
                throw new Exception("Failed to read BGRA pixels into data array.");
            }

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
}