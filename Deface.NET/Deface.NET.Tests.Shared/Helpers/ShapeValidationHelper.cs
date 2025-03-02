using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Tests.Shared.Helpers;

public class ShapeValidationHelper
{
    internal static void ValidateRectangle(Frame frame, DetectedObject detectedObject, Action<PixelData> action)
    {
        var nativeElement = (SKBitmap)frame;

        ValidateRectangleBaseMethod
        (
            nativeElement,
            detectedObject.X1,
            detectedObject.Y1,
            detectedObject.X2,
            detectedObject.Y2,
            action
        );
    }

    internal static void ValidateRectangle(SKBitmap bitmap, int x1, int y1, int x2, int y2, Action<PixelData> action) =>
        ValidateRectangleBaseMethod(bitmap, x1, y1, x2, y2, action);

    private static void ValidateRectangleBaseMethod(SKBitmap bitmap, int x1, int y1, int x2, int y2, Action<PixelData> action)
    {
        int maxX = Math.Max(x1, x2);
        int minX = Math.Min(x1, x2);
        int maxY = Math.Max(y1, y2);
        int minY = Math.Min(y1, y2);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                ValidatePixel(x, y, action, bitmap);
            }
        }
    }

    private static void ValidatePixel(int x, int y, Action<PixelData> action, SKBitmap nativeElement)
    {
        var pixel = nativeElement.GetPixel(x, y);
        PixelData pixelData = new(x, y, pixel.Red, pixel.Green, pixel.Blue);

        action(pixelData);
    }
}
