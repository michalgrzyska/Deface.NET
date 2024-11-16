using Deface.NET.Graphics;
using Deface.NET.Graphics.Drawers;
using Deface.NET.ObjectDetection;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics;

[Collection(nameof(SettingsCollection))]
public class GaussianBlurShapeDrawerUnitTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    private readonly DetectedObject Object1 = new(10, 10, 100, 100, 1, IsEnlarged: true);
    private readonly DetectedObject Object2 = new(110, 110, 200, 200, 1, IsEnlarged: true);
    private readonly DetectedObject Object3 = new(210, 210, 400, 400, 1, IsEnlarged: true);

    [Fact]
    public void X()
    {
        Settings settings = GetSettings(AnonimizationShape.Rectangle);
        Frame frame = TestFrameHelper.GetTestFrameWithMesh();

        GaussianBlurShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, [Object1]);

        result.SaveTo("C:/DefaceTest/blur.png", ImageFormat.Png);

    }

    private Settings GetSettings(AnonimizationShape shape)
    {
        return _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = shape;
            x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
        });
    }
}
