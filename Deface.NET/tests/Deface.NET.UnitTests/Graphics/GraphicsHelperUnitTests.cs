using Deface.NET.Graphics;
using Deface.NET.UnitTests.Graphics.Helpers;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics;

public class GraphicsHelperUnitTests
{
    [Fact]
    public void ConvertBgrToRgba_ShouldConvertBgrToRgbaWithOpaqueAlpha()
    {
        // Arrange

        var width = 2;
        var height = 2;

        byte[] bgrData =
        [
            255, 0, 0,
            0, 255, 0,
            0, 0, 255,
            255, 255, 0
        ];

        byte[] expectedRgbaData =
        [
            0, 0, 255, 255, // B
            0, 255, 0, 255, // G
            255, 0, 0, 255, // R
            0, 255, 255, 255 // Y
        ];

        // Act

        var rgbaData = GraphicsHelper.ConvertBgrToRgba(bgrData, width, height);

        // Assert

        rgbaData.Should().BeEquivalentTo(expectedRgbaData);
    }

    [Fact]
    public void GetBgraBitmapFromBytes_ShouldCreateBgra8888BitmapWithPremultipliedAlpha()
    {
        // Arrange
        var width = 2;
        var height = 2;

        byte[] rgbaData =
        [
            0, 0, 255, 255,
            0, 255, 0, 255,
            255, 0, 0, 255,
            0, 255, 255, 255
        ];

        // Act

        using var bitmap = GraphicsHelper.GetBgraBitmapFromRawBytes(rgbaData, width, height);

        // Assert

        bitmap.Width.Should().Be(width);
        bitmap.Height.Should().Be(height);
        bitmap.ColorType.Should().Be(SKColorType.Bgra8888);
        bitmap.AlphaType.Should().Be(SKAlphaType.Premul);

        bitmap.GetPixel(0, 0).ShouldBe(255, 0, 0);
        bitmap.GetPixel(1, 0).ShouldBe(0, 255, 0);
        bitmap.GetPixel(0, 1).ShouldBe(0, 0, 255);
        bitmap.GetPixel(1, 1).ShouldBe(255, 255, 0);
    }
}