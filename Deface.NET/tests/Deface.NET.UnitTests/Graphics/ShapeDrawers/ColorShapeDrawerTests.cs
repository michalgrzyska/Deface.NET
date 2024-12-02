using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Graphics.ShapeDrawers;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics.ShapeDrawers;

[Collection(nameof(SettingsCollection))]
public class ColorShapeDrawerTests(SettingsFixture settingsFixture)
    : ShapeDrawerTestsBase(settingsFixture)
{
    protected override AnonimizationMethod AnonimizationMethod => AnonimizationMethod.Color(255, 0, 0);

    internal override IShapeDrawer GetShapeDrawer(Settings settings)
    {
        return new ColorShapeDrawer(settings);
    }

    internal override Frame GetTestFrame()
    {
        return TestFrameHelper.GetTestFrame();
    }

    internal override void ValidateWholeFramePixel(PixelData pixel)
    {
        pixel.ShouldBe(255, 255, 255);
    }

    internal override void ValidatePixel(PixelData pixel, Settings settings)
    {
        var color = settings.AnonimizationMethod.ColorValue!;
        pixel.ShouldBe(color);
    }
}
