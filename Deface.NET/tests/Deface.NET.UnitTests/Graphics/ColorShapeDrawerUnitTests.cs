using Deface.NET.Configuration;
using Deface.NET.Graphics;
using Deface.NET.Graphics.Drawers;
using Deface.NET.ObjectDetection;
using Deface.NET.UnitTests._TestsConfig;
using Deface.NET.UnitTests.Graphics.Helpers;
using FluentAssertions;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics;

[Collection(nameof(SettingsCollection))]
public class ColorShapeDrawerUnitTests(SettingsFixture settingsFixture)
{
    private readonly SettingsFixture _settingsFixture = settingsFixture;

    private readonly DetectedObject Object1 = new(10, 10, 100, 100, 1, IsEnlarged: true);
    private readonly DetectedObject Object2 = new(110, 110, 200, 200, 1, IsEnlarged: true);
    private readonly DetectedObject Object3 = new(210, 210, 400, 400, 1, IsEnlarged: true);

    private readonly AnonimizationMethod TestAnomizationMethod = AnonimizationMethod.Color(255, 0, 0);

    [Fact]
    public void DrawObject_NoObjects_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Rectangle);
        Frame frame = TestFrameHelper.GetTestFrame();
        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, []);

        ShapeTestHelper.ValidateWholeFrame(frame, pixel =>
        {
            pixel.R.Should().Be(255);
            pixel.G.Should().Be(255);
            pixel.B.Should().Be(255);
        });
    }

    [Fact]
    public void DrawObject_SingleRectangle_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Rectangle);
        Frame frame = TestFrameHelper.GetTestFrame();

        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, [Object1]);

        ShapeTestHelper.ValidateRectangle(result, Object1, pixel =>
        {
            ValidatePixelColor(pixel, settings.AnonimizationMethod.ColorValue!);
        });
    }

    [Fact]
    public void DrawObject_SingleEllipse_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Ellipse);
        Frame frame = TestFrameHelper.GetTestFrame();

        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, [Object1]);

        ShapeTestHelper.ValidateEllipse(result, Object1, pixel =>
        {
            ValidatePixelColor(pixel, settings.AnonimizationMethod.ColorValue!);
        });
    }

    [Fact]
    public void DrawObject_MultipleEllipses_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Ellipse);
        Frame frame = TestFrameHelper.GetTestFrame();
        List<DetectedObject> objects = [Object1, Object2, Object3];

        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, objects);

        foreach (var obj in objects)
        {
            ShapeTestHelper.ValidateEllipse(result, obj, pixel =>
            {
                ValidatePixelColor(pixel, settings.AnonimizationMethod.ColorValue!);
            });
        }
    }

    [Fact]
    public void DrawObject_MultipleRectangles_DrawnCorrectly()
    {
        Settings settings = GetSettings(AnonimizationShape.Rectangle);
        Frame frame = TestFrameHelper.GetTestFrame();
        List<DetectedObject> objects = [Object1, Object2, Object3];

        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, objects);

        foreach (var obj in objects)
        {
            ShapeTestHelper.ValidateRectangle(result, obj, pixel =>
            {
                ValidatePixelColor(pixel, settings.AnonimizationMethod.ColorValue!);
            });
        }
    }

    private Settings GetSettings(AnonimizationShape shape)
    {
        return _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = shape;
            x.AnonimizationMethod = TestAnomizationMethod;
        });
    }

    private static void ValidatePixelColor(PixelData pixel, Color color)
    {
        pixel.R.Should().Be(color.R);
        pixel.G.Should().Be(color.G);
        pixel.B.Should().Be(color.B);
    }
}
