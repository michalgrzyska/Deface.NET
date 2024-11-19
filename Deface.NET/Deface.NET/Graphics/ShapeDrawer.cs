using Deface.NET.Configuration;
using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Effects;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.ObjectDetection;

namespace Deface.NET.Graphics;

internal class ShapeDrawer : IShapeDrawer
{
    private readonly IEffectShape _shapeEffect;

    public ShapeDrawer(IScopedSettingsProvider settingsProvider)
    {
        var settings = settingsProvider.Settings;

        _shapeEffect = settings.AnonimizationMethod.Type switch
        {
            AnonimizationType.Color => new ColorShapeEffect(settings),
            AnonimizationType.GaussianBlur => new GaussianBlurShapeEffect(settings),
            AnonimizationType.Mosaic => new MosaicShapeEffect(settings),
            _ => throw new NotImplementedException()
        };
    }

    public Frame DrawShapes(Frame frame, List<DetectedObject> objects) => _shapeEffect.Draw(frame, objects);
}
