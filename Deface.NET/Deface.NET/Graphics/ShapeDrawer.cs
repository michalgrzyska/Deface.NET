using Deface.NET.Configuration;
using Deface.NET.Graphics.Drawers;
using Deface.NET.ObjectDetection;
using SkiaSharp;

namespace Deface.NET.Graphics;

internal static class ShapeDrawer
{
    public static SKBitmap DrawShapes(SKBitmap bitmap, List<DetectedObject> objects, Settings settings)
    {
        IDrawer drawer = settings.AnonimizationMethod.Type switch
        {
            AnonimizationType.Color => new ColorShapeDrawer(),
            AnonimizationType.GaussianBlur => new GaussianBlurShapeDrawer(),
            AnonimizationType.Mosaic => new MosaicShapeDrawer(),
            _ => throw new NotImplementedException()
        };

        return drawer.Draw(bitmap, objects, settings);
    }
}
