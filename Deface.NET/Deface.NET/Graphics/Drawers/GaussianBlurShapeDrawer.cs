using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.Drawers;

internal class GaussianBlurShapeDrawer : IShapeDrawer
{
    public SKBitmap Draw(SKBitmap bitmap, List<DetectedObject> objects, Settings settings)
    {
        SKImageInfo imageInfo = new(bitmap.Width, bitmap.Height);
        using SKSurface surface = SKSurface.Create(imageInfo);
        SKCanvas canvas = surface.Canvas;

        canvas.DrawBitmap(bitmap, 0, 0);

        using SKImageFilter blurFilter = SKImageFilter.CreateBlur(10.0f, 10.0f); // TODO
        using SKPaint paint = new() { ImageFilter = blurFilter };

        foreach (DetectedObject obj in objects)
        {
            canvas.Save();

            DrawShape(bitmap, paint, settings, obj, canvas);

            canvas.Restore();
        }

        using SKImage snapshot = surface.Snapshot();
        SKBitmap resultBitmap = new(bitmap.Width, bitmap.Height);

        snapshot.ReadPixels(resultBitmap.Info, resultBitmap.GetPixels(), resultBitmap.RowBytes, 0, 0);
        return resultBitmap;
    }

    private static void DrawShape(SKBitmap bitmap, SKPaint paint, Settings settings, DetectedObject obj, SKCanvas canvas)
    {
        SKRect blurRect = new(obj.X1, obj.Y1, obj.X2, obj.Y2);

        if (settings.AnonimizationShape == AnonimizationShape.Rectangle)
        {
            canvas.ClipRect(blurRect);
            canvas.DrawBitmap(bitmap, 0, 0, paint);
        }
        else if (settings.AnonimizationShape == AnonimizationShape.Ellipse)
        {
            using SKPath path = new();

            path.AddOval(blurRect);
            canvas.ClipPath(path);
            canvas.DrawBitmap(bitmap, 0, 0, paint);
        }
    }
}
