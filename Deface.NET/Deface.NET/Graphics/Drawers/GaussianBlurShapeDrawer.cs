using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.Drawers;

internal class GaussianBlurShapeDrawer : IShapeDrawer
{
    public Frame Draw(Frame frame, List<DetectedObject> objects, Settings settings)
    {
        var bitmap = frame.GetNativeElement();
        float blurFactor = CalculateBlurFactor(frame);

        SKImageInfo imageInfo = new(bitmap.Width, bitmap.Height);
        using SKSurface surface = SKSurface.Create(imageInfo);
        SKCanvas canvas = surface.Canvas;

        canvas.DrawBitmap(bitmap, 0, 0);

        using SKImageFilter blurFilter = SKImageFilter.CreateBlur(blurFactor, blurFactor);
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

        frame.UpdateNativeElement(resultBitmap);
        return frame;
    }

    private static float CalculateBlurFactor(Frame frame)
    {
        var longerSide = Math.Max(frame.Width, frame.Height);
        return longerSide / 100;
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
