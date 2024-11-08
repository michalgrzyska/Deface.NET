using Deface.NET.Configuration;
using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Drawers;
using Deface.NET.ObjectDetection;

namespace Deface.NET.Graphics;

internal class ShapeDrawingService(ScopedSettingsProvider settingsProvider)
{
    private readonly Settings _settings = settingsProvider.Settings;

    public Frame DrawShapes(Frame frame, List<DetectedObject> objects)
    {
        IShapeDrawer drawer = _settings.AnonimizationMethod.Type switch
        {
            AnonimizationType.Color => new ColorShapeDrawer(_settings),
            AnonimizationType.GaussianBlur => new GaussianBlurShapeDrawer(_settings),
            AnonimizationType.Mosaic => new MosaicShapeDrawer(_settings),
            _ => throw new NotImplementedException()
        };

        return drawer.Draw(frame, objects);
    }
}
