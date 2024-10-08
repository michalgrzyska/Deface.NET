using SkiaSharp;
using System.Runtime.InteropServices;

namespace Deface.NET.Graphics;

internal static class GraphicsHelper
{
    public static SKBitmap CreateBitmapFromBgra(byte[] bgraData, int width, int height)
    {
        SKBitmap bitmap = new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
        var pixelPtr = bitmap.GetPixels();

        Marshal.Copy(bgraData, 0, pixelPtr, bgraData.Length);

        return bitmap;
    }
}
