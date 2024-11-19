using Deface.NET.Configuration;
using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Drawers;
using Deface.NET.ObjectDetection;

namespace Deface.NET.Graphics;

internal class ShapeDrawingService
{
    private readonly IShapeDrawer _shapeDrawer;

    public ShapeDrawingService(IScopedSettingsProvider settingsProvider)
    {
        var settings = settingsProvider.Settings;

        _shapeDrawer = settings.AnonimizationMethod.Type switch
        {
            AnonimizationType.Color => new ColorShapeDrawer(settings),
            AnonimizationType.GaussianBlur => new GaussianBlurShapeDrawer(settings),
            AnonimizationType.Mosaic => new MosaicShapeDrawer(settings),
            _ => throw new NotImplementedException()
        };
    }

    public Frame DrawShapes(Frame frame, List<DetectedObject> objects) => _shapeDrawer.Draw(frame, objects);
}
