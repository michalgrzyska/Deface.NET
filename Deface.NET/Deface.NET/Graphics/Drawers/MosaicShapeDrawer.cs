using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.Drawers;

internal class MosaicShapeDrawer(Settings settings) : IShapeDrawer
{
    private readonly Settings _settings = settings;

    public Frame Draw(Frame frame, List<DetectedObject> objects)
    {
        var bitmap = frame.GetNativeElement();
        int mosaicSize = GetMosaicSize(frame);

        using SKCanvas canvas = new(bitmap);

        foreach (var obj in objects)
        {
            var rect = new SKRectI(obj.X1, obj.Y1, obj.X2, obj.Y2);

            using SKBitmap roiBitmap = new(rect.Width, rect.Height);
            using SKCanvas roiCanvas = new(roiBitmap);

            roiCanvas.DrawBitmap(bitmap, rect, new SKRect(0, 0, roiBitmap.Width, roiBitmap.Height));

            SKBitmap mosaicBitmap = CreateMosaic(roiBitmap, mosaicSize);

            if (_settings.AnonimizationShape == AnonimizationShape.Rectangle)
            {
                canvas.Save();
                canvas.ClipRect(rect);
                canvas.DrawBitmap(mosaicBitmap, rect);
                canvas.Restore();
            }
            else if (_settings.AnonimizationShape == AnonimizationShape.Ellipse)
            {
                var path = new SKPath();
                path.AddOval(new SKRect(obj.X1, obj.Y1, obj.X2, obj.Y2));

                canvas.Save();
                canvas.ClipPath(path);
                canvas.DrawBitmap(mosaicBitmap, rect);
                canvas.Restore();
            }
        }

        return frame;
    }

    private static int GetMosaicSize(Frame frame)
    {
        var longerSide = Math.Max(frame.Width, frame.Height);
        return longerSide / 50;
    }

    private static SKBitmap CreateMosaic(SKBitmap bitmap, int mosaicSize)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        int mosaicWidth = width / mosaicSize;
        int mosaicHeight = height / mosaicSize;

        SKBitmap mosaicBitmap = new(width, height);
        using SKCanvas canvas = new(mosaicBitmap);
        using var smallBitmap = bitmap.Resize(new SKImageInfo(mosaicWidth, mosaicHeight), SKFilterQuality.None);

        canvas.DrawBitmap(smallBitmap, new SKRect(0, 0, width, height));

        return mosaicBitmap;
    }
}
