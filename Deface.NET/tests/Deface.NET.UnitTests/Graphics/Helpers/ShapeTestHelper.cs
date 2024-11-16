using Deface.NET.Graphics;
using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics.Helpers;

internal static class ShapeTestHelper
{
    // Ellipse drawn by library may have slightly different edges so we need to allow some tolerance.
    private const double EllipseShrinkFactor = 2.0;

    public static void ValidateRectangle(Frame frame, DetectedObject detectedObject, Action<PixelData> action)
    {
        var nativeElement = frame.GetNativeElement();

        for (int y = detectedObject.Y1; y < detectedObject.Y2; y++)
        {
            for (int x = detectedObject.X1; x < detectedObject.X2; x++)
            {
                ValidatePixel(x, y, action, nativeElement);
            }
        }
    }

    public static void ValidateEllipse(Frame frame, DetectedObject detectedObject, Action<PixelData> action)
    {
        var nativeElement = frame.GetNativeElement();
        var pixelsInEllipse = GetPointsInsideEllipse(detectedObject);

        foreach (var (x, y) in pixelsInEllipse)
        {
            ValidatePixel(x, y, action, nativeElement);
        }
    }

    private static List<(int X, int Y)> GetPointsInsideEllipse(DetectedObject detectedObject)
    {
        int minX = Math.Min(detectedObject.X1, detectedObject.X2);
        int minY = Math.Min(detectedObject.Y1, detectedObject.Y2);
        int maxX = Math.Max(detectedObject.X1, detectedObject.X2);
        int maxY = Math.Max(detectedObject.Y1, detectedObject.Y2);

        int rectWidth = maxX - minX;
        int rectHeight = maxY - minY;

        double xc = minX + rectWidth / 2.0;
        double yc = minY + rectHeight / 2.0;
        double a = rectWidth / 2.0;
        double b = rectHeight / 2.0;

        double shrunkA = a - EllipseShrinkFactor;
        double shrunkB = b - EllipseShrinkFactor;

        if (shrunkA <= 0 || shrunkB <= 0)
        {
            throw new ArgumentException("Półosie elipsy po pomniejszeniu są mniejsze lub równe zeru.");
        }

        List<(int X, int Y)> pointsInsideShrunkEllipse = [];

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (IsInsideEllipse(x, y, xc, yc, shrunkA, shrunkB))
                {
                    pointsInsideShrunkEllipse.Add((x, y));
                }
            }
        }

        return pointsInsideShrunkEllipse;
    }

    private static bool IsInsideEllipse(double x, double y, double xc, double yc, double a, double b)
    {
        return Math.Pow((x - xc) / a, 2) + Math.Pow((y - yc) / b, 2) <= 1;
    }

    private static void ValidatePixel(int x, int y, Action<PixelData> action, SKBitmap nativeElement)
    {
        var pixel = nativeElement.GetPixel(x, y);
        PixelData pixelData = new(x, y, pixel.Red, pixel.Green, pixel.Blue);

        action(pixelData);
    }
}
