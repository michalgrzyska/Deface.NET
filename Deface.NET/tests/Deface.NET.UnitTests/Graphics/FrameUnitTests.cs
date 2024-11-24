using Deface.NET.Graphics;
using Deface.NET.Graphics.Models;
using Deface.NET.UnitTests.Graphics.Helpers;
using SkiaSharp;

namespace Deface.NET.UnitTests.Graphics;

public class FrameUnitTests
{
    [Theory]
    [InlineData(TestResources.TestResources.PhotoRed, 200, 200)]
    [InlineData(TestResources.TestResources.Photo1, 1280, 946)]
    public void FrameHasCorrectSize(string path, int width, int height)
    {
        Frame frame = TestFrameHelper.GetTestFrame(path);

        frame.Width.Should().Be(width);
        frame.Height.Should().Be(height);
    }

    [Fact]
    public void New_FromBgrData_FrameCreatedCorrectly()
    {
        var bgrBitmap = CreateRedRgbBitmap(200, 200);

        Frame frame = (Frame)GraphicsHelper.GetBgraBitmapFromRawBytes(bgrBitmap, 200, 200);

        frame.GetPixel(0, 0).ShouldBe(0, 0, 255);
    }

    [Fact]
    public void New_FromStream_CreatedCorrectly()
    {
        using FileStream fileStream = new(TestResources.TestResources.PhotoRed, FileMode.Open);

        var action = () => new Frame(fileStream);

        action.Should().NotThrow();
    }

    [Fact]
    public void GetPixel_ReturnsCorrectValue()
    {
        using FileStream fileStream = new(TestResources.TestResources.PhotoRed, FileMode.Open);
        Frame frame = new(fileStream);

        frame.GetPixel(0, 0).ShouldBe(255, 0, 0);
    }

    [Fact]
    public void GetNativeElement_WorksCorrectly()
    {
        Frame frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        var nativeElement = (SKBitmap)frame;

        nativeElement.Width.Should().Be(200);
        nativeElement.Height.Should().Be(200);
        nativeElement.GetPixel(0, 0).Red.Should().Be(255);
    }

    [Fact]
    public void ToByteArray_Png_IsReturnDataOK()
    {
        Frame frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        var byteArray = frame.ToByteArray(ImageFormat.Png);

        var bitmap = SKBitmap.Decode(byteArray);

        bitmap.Width.Should().Be(frame.Width);
        bitmap.Height.Should().Be(frame.Height);
    }

    [Fact]
    public void ToByteArray_IsReturnDataOK()
    {
        Frame frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        var byteArray = frame.ToByteArray();
        var bitmap = GraphicsHelper.GetBgraBitmapFromRawBytes(byteArray, frame.Width, frame.Height);

        bitmap.Width.Should().Be(frame.Width);
        bitmap.Height.Should().Be(frame.Height);
        bitmap.GetPixel(0, 0).Blue.Should().Be(255);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(40)]
    [InlineData(90)]
    [InlineData(100)]
    public void ToByteArray_Jpg_IsReturnDataOK(int quality)
    {
        Frame frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        var byteArray = frame.ToByteArray(ImageFormat.Jpeg(quality));

        var bitmap = SKBitmap.Decode(byteArray);

        bitmap.Width.Should().Be(frame.Width);
        bitmap.Height.Should().Be(frame.Height);
    }

    private static byte[] CreateRedRgbBitmap(int width, int height)
    {
        int bytesPerPixel = 3;
        byte[] bgrData = new byte[width * height * bytesPerPixel];

        for (int i = 0; i < bgrData.Length; i += bytesPerPixel)
        {
            bgrData[i] = 255;     // Blue
            bgrData[i + 1] = 0; // Green
            bgrData[i + 2] = 0; // Red
        }

        return bgrData;
    }

    [Fact]
    public void AsRescaledWithPadding_ShouldReturnBitmap_WithCorrectDimensions()
    {
        // Arrange
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);
        int targetWidth = 300;
        int targetHeight = 350;

        // Act
        var result = frame.AsRescaledWithPadding(targetWidth, targetHeight);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(targetWidth, result.Width);
        Assert.Equal(targetHeight, result.Height);
    }

    [Fact]
    public void AsRescaledWithPadding_ShouldCenterImage_WithCorrectPadding()
    {
        // Arrange
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);
        int targetWidth = 300;
        int targetHeight = 350;

        float scale = Math.Min((float)targetWidth / 200, (float)targetHeight / 200);
        int scaledWidth = (int)(200 * scale);
        int scaledHeight = (int)(200 * scale);
        int expectedOffsetX = (targetWidth - scaledWidth) / 2;
        int expectedOffsetY = (targetHeight - scaledHeight) / 2;

        // Act
        var result = frame.AsRescaledWithPadding(targetWidth, targetHeight);

        // Assert
        result.GetPixel(0, 0).ShouldBe(0, 0, 0);
        result.GetPixel(targetWidth - 1, targetHeight - 1).ShouldBe(0, 0, 0);

        result.GetPixel(expectedOffsetX + 1, expectedOffsetY + 1).ShouldBe(255, 0, 0);
        result.GetPixel(expectedOffsetX + scaledWidth - 1, expectedOffsetY + scaledHeight - 1).ShouldBe(255, 0, 0);
    }

    [Fact]
    public void AsRescaledWithPadding_ShouldReturnBlackPadding()
    {
        // Arrange
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);
        int targetWidth = 300;
        int targetHeight = 350;

        // Act
        var result = frame.AsRescaledWithPadding(targetWidth, targetHeight);

        // Assert
        result.GetPixel(0, 0).ShouldBe(0, 0, 0);
        result.GetPixel(targetWidth - 1, targetHeight - 1).ShouldBe(0, 0, 0);
    }
}

file static class Extensions
{
    public static void ShouldBe(this Pixel pixel, byte r, byte g, byte b)
    {
        pixel.R.Should().Be(r);
        pixel.G.Should().Be(g);
        pixel.B.Should().Be(b);
    }
}
