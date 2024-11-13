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

    [Fact]
    public void DrawObject_Rectangle_DrawnCorrectly()
    {
        var settings = _settingsFixture.WithAction(x =>
        {
            x.AnonimizationShape = AnonimizationShape.Rectangle;
            x.AnonimizationMethod = AnonimizationMethod.Color(255, 0, 0);
        });

        var x1 = 10;
        var y1 = 10;
        var x2 = 100;
        var y2 = 100;

        DetectedObject detectedObject = new(x1, y1, x2, y2, 1, IsEnlarged: true);
        Frame frame = TestFrameHelper.GetTestFrame();

        ColorShapeDrawer drawer = new(settings);
        Frame result = drawer.Draw(frame, [detectedObject]);

        var nativeElement = result.GetNativeElement();

        for (int i = y1; i < y2; i++)
        {
            for (int j = x1; j < x2; j++)
            {
                var pixel = nativeElement.GetPixel(j, i);
                
                pixel.Red.Should().Be(255);
                pixel.Green.Should().Be(0);
                pixel.Blue.Should().Be(0);
            }
        }
    }
}
