using Deface.NET.VideoIO;
using SkiaSharp;
using System.Runtime.InteropServices;

namespace Deface.NET;

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
}
