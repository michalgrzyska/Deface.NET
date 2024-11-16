using Deface.NET.Graphics;
using Deface.NET.Graphics.Drawers;
using Deface.NET.ObjectDetection;
using Deface.NET.UnitTests._TestsConfig;
using FluentAssertions;

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
        var settings = _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = AnonimizationShape.Rectangle;
            x.AnonimizationMethod = TestAnomizationMethod;
        });

        Frame frame = TestFrameHelper.GetTestFrame();
        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, []);

        var nativeElement = frame.GetNativeElement();

        for (int y = 0; y < frame.Height; y++) 
        {
            for (int x = 0; x < frame.Width; x++) 
            {
                var pixel = nativeElement.GetPixel(x, y);

                pixel.Red.Should().Be(255);
                pixel.Green.Should().Be(255);
                pixel.Blue.Should().Be(255);
            }
        }
    }

    [Fact]
    public void DrawObject_SingleRectangle_DrawnCorrectly()
    {
        var settings = _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = AnonimizationShape.Rectangle;
            x.AnonimizationMethod = TestAnomizationMethod;
        });

        Frame frame = TestFrameHelper.GetTestFrame();

        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, [Object1]);

        ShapeTestHelper.ValidateRectangle(result, Object1, settings.AnonimizationMethod.ColorValue!);
    }

    [Fact]
    public void DrawObject_SingleEllipse_DrawnCorrectly()
    {
        var settings = _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = AnonimizationShape.Ellipse;
            x.AnonimizationMethod = TestAnomizationMethod;
        });

        Frame frame = TestFrameHelper.GetTestFrame();

        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, [Object1]);

        ShapeTestHelper.ValidateEllipse(result, Object1, settings.AnonimizationMethod.ColorValue!);
    }
}
