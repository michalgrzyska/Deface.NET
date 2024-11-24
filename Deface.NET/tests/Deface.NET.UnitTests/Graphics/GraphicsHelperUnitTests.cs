using Deface.NET.Graphics;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics;

public class GraphicsHelperUnitTests
{
    [Fact]
    public void ConvertBgrToRgba_ShouldConvertBgrToRgbaWithOpaqueAlpha()
    {
        // Arrange

        int width = 2;
        int height = 2;
        byte[] bgrData =
        [
            255, 0, 0,
            0, 255, 0,
            0, 0, 255,
            255, 255, 0
        ];

        byte[] expectedRgbaData =
        [
            0, 0, 255, 255,  // Blue pixel
            0, 255, 0, 255,  // Green pixel
            255, 0, 0, 255,  // Red pixel
            0, 255, 255, 255 // Yellow pixel
        ];

        // Act
        byte[] rgbaData = GraphicsHelper.ConvertBgrToRgba(bgrData, width, height);

        // Assert
        rgbaData.Should().BeEquivalentTo(expectedRgbaData);
    }

    [Fact]
    public void GetBgraBitmapFromBytes_ShouldCreateBgra8888BitmapWithPremultipliedAlpha()
    {
        // Arrange
        int width = 2;
        int height = 2;
        byte[] rgbaData =
        [
            0, 0, 255, 255,
            0, 255, 0, 255,
            255, 0, 0, 255,
            0, 255, 255, 255
        ];

        // Act
        using SKBitmap bitmap = GraphicsHelper.GetBgraBitmapFromRawBytes(rgbaData, width, height);

        // Assert
        bitmap.Width.Should().Be(width);
        bitmap.Height.Should().Be(height);
        bitmap.ColorType.Should().Be(SKColorType.Bgra8888);
        bitmap.AlphaType.Should().Be(SKAlphaType.Premul);

        bitmap.GetPixel(0, 0).Should().Be(new SKColor(255, 0, 0, 255));
        bitmap.GetPixel(1, 0).Should().Be(new SKColor(0, 255, 0, 255));
        bitmap.GetPixel(0, 1).Should().Be(new SKColor(0, 0, 255, 255));
        bitmap.GetPixel(1, 1).Should().Be(new SKColor(255, 255, 0, 255));
    }
}