using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Graphics.ShapeDrawers;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics.ShapeDrawers;

[Collection(nameof(SettingsCollection))]
public class GaussianBlurShapeDrawerTests(SettingsFixture settingsFixture)
    : ShapeDrawerTestsBase(settingsFixture)
{
    protected override AnonimizationMethod AnonimizationMethod => AnonimizationMethod.GaussianBlur;

    internal override IShapeDrawer GetShapeDrawer(Settings settings)
    {
        return new GaussianBlurShapeDrawer(settings);
    }

    internal override Frame GetTestFrame()
    {
        return TestFrameHelper.GetTestFrameWithMesh();
    }

    internal override void ValidateWholeFramePixel(PixelData pixel)
    {
        pixel.R.Should().BeOneOf(0, 255);
        pixel.G.Should().BeOneOf(0, 255);
        pixel.B.Should().BeOneOf(0, 255);
    }

    internal override void ValidatePixel(PixelData pixel, Settings settings)
    {
        var pixelGrayValue = (pixel.R + pixel.G + pixel.B) / 3.0;
        pixelGrayValue.Should().BeGreaterThan(0).And.BeLessThan(255);
    }
}
