using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics.ShapeDrawers;

internal class MosaicShapeDrawer(Settings settings) : IShapeDrawer
{
    private readonly Settings _settings = settings;

    private const int MosaicDivisionFactor = 20;

    public Frame Draw(Frame frame, List<DetectedObject> objects)
    {
        var bitmap = (SKBitmap)frame;
        var (mosaicW, mosaicH) = GetMosaicSize(frame);

        using SKCanvas canvas = new(bitmap);

        foreach (var obj in objects)
        {
            var rect = new SKRectI(obj.X1, obj.Y1, obj.X2, obj.Y2);

            using SKBitmap roiBitmap = new(rect.Width, rect.Height);
            using SKCanvas roiCanvas = new(roiBitmap);

            roiCanvas.DrawBitmap(bitmap, rect, new SKRect(0, 0, roiBitmap.Width, roiBitmap.Height));

            SKBitmap mosaicBitmap = CreateMosaic(roiBitmap, mosaicW, mosaicH);

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

    internal static (int SizeW, int SizeH) GetMosaicSize(Frame frame)
    {
        var longerSide = Math.Max(frame.Width, frame.Height);
        var mosaicSize = longerSide / MosaicDivisionFactor;

        int mosaicWidth = frame.Width / mosaicSize;
        int mosaicHeight = frame.Height / mosaicSize;

        return (mosaicWidth, mosaicHeight);
    }

    private static SKBitmap CreateMosaic(SKBitmap bitmap, int mosaicW, int mosaicH)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        SKBitmap mosaicBitmap = new(width, height);
        using SKCanvas canvas = new(mosaicBitmap);
        using var smallBitmap = bitmap.Resize(new SKImageInfo(mosaicW, mosaicH), SKFilterQuality.None);

        if (smallBitmap != null)
        {
            canvas.DrawBitmap(smallBitmap, new SKRect(0, 0, width, height));
        }

        return mosaicBitmap;
    }
}
