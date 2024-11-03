using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.Drawers;

internal class ColorShapeDrawer : IShapeDrawer
{
    public Frame Draw(Frame frame, List<DetectedObject> objects, Settings settings)
    {
        var bitmap = frame.GetNativeElement();
        var color = settings.AnonimizationMethod.ColorValue!;

        using SKCanvas canvas = new(bitmap);

        using SKPaint paint = new()
        {
            Style = SKPaintStyle.Fill,
            Color = new SKColor(color.R, color.G, color.B)
        };

        foreach (var obj in objects)
        {
            DrawObject(canvas, paint, obj, settings);
        }

        frame.UpdateNativeElement(bitmap);
        return frame;
    }

    private static void DrawObject(SKCanvas canvas, SKPaint paint, DetectedObject obj, Settings settings)
    {
        SKRect rect = new(obj.X1, obj.Y1, obj.X2, obj.Y2);

        if (settings.AnonimizationShape == AnonimizationShape.Rectangle)
        {
            canvas.DrawRect(rect, paint);
        }
        else if (settings.AnonimizationShape == AnonimizationShape.Ellipse)
        {
            canvas.DrawOval(rect, paint);
        }
    }
}
