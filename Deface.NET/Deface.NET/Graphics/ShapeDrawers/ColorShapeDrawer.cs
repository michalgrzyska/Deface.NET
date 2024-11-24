using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.ShapeDrawers;

internal class ColorShapeDrawer(Settings settings) : IShapeDrawer
{
    private readonly Settings _settings = settings;

    public Frame Draw(Frame frame, List<DetectedObject> objects)
    {
        var bitmap = (SKBitmap)frame;
        var color = _settings.AnonimizationMethod.ColorValue!;

        using SKCanvas canvas = new(bitmap);

        using SKPaint paint = new()
        {
            Style = SKPaintStyle.Fill,
            Color = new SKColor(color.R, color.G, color.B)
        };

        foreach (var obj in objects)
        {
            DrawObject(canvas, paint, obj);
        }

        return frame;
    }

    private void DrawObject(SKCanvas canvas, SKPaint paint, DetectedObject obj)
    {
        SKRect rect = new(obj.X1, obj.Y1, obj.X2, obj.Y2);

        if (_settings.AnonimizationShape == AnonimizationShape.Rectangle)
        {
            canvas.DrawRect(rect, paint);
        }
        else if (_settings.AnonimizationShape == AnonimizationShape.Ellipse)
        {
            canvas.DrawOval(rect, paint);
        }
    }
}
