using Deface.NET.Graphics.Interfaces;
using Deface.NET.Graphics.Models;
using Deface.NET.Graphics.ShapeDrawers;
using Deface.NET.Tests.Shared.Helpers;
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
        byte white = 255;
        byte black = 0;

        pixel.R.ShouldBeOneOf(black, white);
        pixel.G.ShouldBeOneOf(black, white);
        pixel.B.ShouldBeOneOf(black, white);
    }

    internal override void ValidatePixel(PixelData pixel, Settings settings)
    {
        var pixelGrayValue = (pixel.R + pixel.G + pixel.B) / 3.0;
        pixelGrayValue.ShouldBeGreaterThan(0);
        pixelGrayValue.ShouldBeLessThan(255);
    }
}
