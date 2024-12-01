using Deface.NET.Configuration;
using Deface.NET.Configuration.Provider;
using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.ShapeDrawers;

namespace Deface.NET.Graphics;

internal class ShapeDrawerProvider(IScopedSettingsProvider settingsProvider) : IShapeDrawerProvider
{
    private readonly Settings _settings = settingsProvider.Settings;

    private IShapeDrawer? _shapeDrawer;

    public IShapeDrawer ShapeDrawer
    {
        get
        {
            _shapeDrawer ??= GetShapeDrawer();
            return _shapeDrawer;
        }
    }

    private IShapeDrawer GetShapeDrawer()
    {
        return _settings.AnonimizationMethod.Type switch
        {
            AnonimizationType.Color => new ColorShapeDrawer(_settings),
            AnonimizationType.GaussianBlur => new GaussianBlurShapeDrawer(_settings),
            AnonimizationType.Mosaic => new MosaicShapeDrawer(_settings),
            _ => throw new NotImplementedException()
        };
    }
}
