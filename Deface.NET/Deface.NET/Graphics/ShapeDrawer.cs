using Deface.NET.Configuration;
using Deface.NET.Graphics.Drawers;
using Deface.NET.ObjectDetection;

namespace Deface.NET.Graphics;

internal static class ShapeDrawer
{
    public static Frame DrawShapes(Frame frame, List<DetectedObject> objects, Settings settings)
    {
        IShapeDrawer drawer = settings.AnonimizationMethod.Type switch
        {
            AnonimizationType.Color => new ColorShapeDrawer(),
            AnonimizationType.GaussianBlur => new GaussianBlurShapeDrawer(),
            AnonimizationType.Mosaic => new MosaicShapeDrawer(),
            _ => throw new NotImplementedException()
        };

        objects = objects.Select(x => x.GetResized(settings.MaskScale)).ToList();

        return drawer.Draw(frame, objects, settings);
    }
}
