using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.Effects;

internal class GaussianBlurShapeDrawer(Settings settings) : IShapeDrawer
{
    private readonly Settings _settings = settings;

    public Frame Draw(Frame frame, List<DetectedObject> objects)
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

            DrawShape(bitmap, paint, obj, canvas);

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

    private void DrawShape(SKBitmap bitmap, SKPaint paint, DetectedObject obj, SKCanvas canvas)
    {
        SKRect blurRect = new(obj.X1, obj.Y1, obj.X2, obj.Y2);

        if (_settings.AnonimizationShape == AnonimizationShape.Rectangle)
        {
            canvas.ClipRect(blurRect);
            canvas.DrawBitmap(bitmap, 0, 0, paint);
        }
        else if (_settings.AnonimizationShape == AnonimizationShape.Ellipse)
        {
            using SKPath path = new();

            path.AddOval(blurRect);
            canvas.ClipPath(path);
            canvas.DrawBitmap(bitmap, 0, 0, paint);
        }
    }
}
