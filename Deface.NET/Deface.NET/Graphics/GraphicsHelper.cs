using SkiaSharp;
using System.Runtime.InteropServices;

namespace Deface.NET.Graphics;

internal static class GraphicsHelper
{
    public static byte[] ConvertBgrToRgba(byte[] bgrData, int width, int height)
    {
        var rgbaData = new byte[width * height * 4];

        for (int j = 0; j < width * height; j++)
        {
            var b = bgrData[j * 3];
            var g = bgrData[j * 3 + 1];
            var r = bgrData[j * 3 + 2];

            rgbaData[j * 4] = r;
            rgbaData[j * 4 + 1] = g;
            rgbaData[j * 4 + 2] = b;
            rgbaData[j * 4 + 3] = 255;
        }

        return rgbaData;
    }

    public static SKBitmap GetBgraBitmapFromRawBytes(byte[] rgbData, int width, int height)
    {
        SKBitmap bitmap = new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
        var handle = GCHandle.Alloc(rgbData, GCHandleType.Pinned);
        var pixelPointer = handle.AddrOfPinnedObject();

        bitmap.InstallPixels(new SKImageInfo(width, height, SKColorType.Bgra8888), pixelPointer, width * 4);
        return bitmap;
    }
}
