using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.ShapeDrawers;

internal class GaussianBlurShapeDrawer(Settings settings) : IShapeDrawer
{
    private readonly Settings _settings = settings;

    public Frame Draw(Frame frame, List<DetectedObject> objects)
    {
        var bitmap = (SKBitmap)frame;
        var blurFactor = CalculateBlurFactor(frame);

        SKImageInfo imageInfo = new(bitmap.Width, bitmap.Height);
        using var surface = SKSurface.Create(imageInfo);
        var canvas = surface.Canvas;

        canvas.DrawBitmap(bitmap, 0, 0);

        using var blurFilter = SKImageFilter.CreateBlur(blurFactor, blurFactor);
        using SKPaint paint = new() { ImageFilter = blurFilter };

        foreach (var obj in objects)
        {
            canvas.Save();

            DrawShape(bitmap, paint, obj, canvas);

            canvas.Restore();
        }

        using var snapshot = surface.Snapshot();
        SKBitmap resultBitmap = new(bitmap.Width, bitmap.Height);

        snapshot.ReadPixels(resultBitmap.Info, resultBitmap.GetPixels(), resultBitmap.RowBytes, 0, 0);

        return (Frame)resultBitmap;
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
