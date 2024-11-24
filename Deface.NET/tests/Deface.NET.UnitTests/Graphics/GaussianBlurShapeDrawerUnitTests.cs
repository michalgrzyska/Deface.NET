using Deface.NET.Graphics.ShapeDrawers;
using Deface.NET.Graphics.Models;
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
    public void DrawObject_NoObjects_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Rectangle);
        Frame frame = TestFrameHelper.GetTestFrameWithMesh();
        GaussianBlurShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, []);

        ShapeTestHelper.ValidateWholeFrame(frame, pixel =>
        {
            pixel.R.Should().BeOneOf(0, 255);
            pixel.G.Should().BeOneOf(0, 255);
            pixel.B.Should().BeOneOf(0, 255);
        });
    }

    [Fact]
    public void DrawObject_SingleRectangle_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Rectangle);
        Frame frame = TestFrameHelper.GetTestFrameWithMesh();
        GaussianBlurShapeDrawer drawer = new(settings);

        Frame result = drawer.Draw(frame, [Object1]);

        ShapeTestHelper.ValidateRectangle(frame, Object1, ValidatePixel);
    }

    [Fact]
    public void DrawObject_SingleEllipse_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Ellipse);
        Frame frame = TestFrameHelper.GetTestFrameWithMesh();
        GaussianBlurShapeDrawer drawer = new(settings);

        Frame result = drawer.Draw(frame, [Object1]);

        ShapeTestHelper.ValidateEllipse(frame, Object1, ValidatePixel);
    }

    [Fact]
    public void DrawObject_MultipleEllipses_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Ellipse);
        Frame frame = TestFrameHelper.GetTestFrameWithMesh();
        GaussianBlurShapeDrawer drawer = new(settings);
        List<DetectedObject> objects = [Object1, Object2, Object3];

        Frame result = drawer.Draw(frame, objects);

        foreach (var obj in objects)
        {
            ShapeTestHelper.ValidateEllipse(frame, obj, ValidatePixel);
        }
    }

    [Fact]
    public void DrawObject_MultipleRectangles_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Rectangle);
        Frame frame = TestFrameHelper.GetTestFrameWithMesh();
        GaussianBlurShapeDrawer drawer = new(settings);
        List<DetectedObject> objects = [Object1, Object2, Object3];

        Frame result = drawer.Draw(frame, objects);

        foreach (var obj in objects)
        {
            ShapeTestHelper.ValidateEllipse(frame, obj, ValidatePixel);
        }
    }

    private Settings GetSettings(AnonimizationShape shape)
    {
        return _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = shape;
            x.AnonimizationMethod = AnonimizationMethod.GaussianBlur;
        });
    }

    private static void ValidatePixel(PixelData pixel)
    {
        var pixelGrayValue = (pixel.R + pixel.G + pixel.B) / 3.0;
        pixelGrayValue.Should().BeGreaterThan(0).And.BeLessThan(255);
    }
}
