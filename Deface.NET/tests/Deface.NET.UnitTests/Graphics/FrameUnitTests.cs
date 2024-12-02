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
        var frame = TestFrameHelper.GetTestFrame(path);
        frame.ShouldBe(width, height);
    }

    [Fact]
    public void New_FromBgrData_FrameCreatedCorrectly()
    {
        var bgrBitmap = CreateRedRgbBitmap(200, 200);

        var frame = (Frame)GraphicsHelper.GetBgraBitmapFromRawBytes(bgrBitmap, 200, 200);

        frame.GetPixel(0, 0).ShouldBe(0, 0, 255);
    }

    [Fact]
    public void GetPixel_ReturnsCorrectValue()
    {
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        frame.GetPixel(0, 0).ShouldBe(255, 0, 0);
    }

    [Fact]
    public void GetNativeElement_WorksCorrectly()
    {
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        var nativeElement = (SKBitmap)frame;

        nativeElement.ShouldBe(200, 200);
        nativeElement.GetPixel(0, 0).ShouldBe(255, 0, 0);
    }

    [Fact]
    public void ToByteArray_Png_IsReturnDataOK()
    {
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);
        var byteArray = frame.ToByteArray(ImageFormat.Png);

        var bitmap = SKBitmap.Decode(byteArray);

        bitmap.ShouldBe(frame.Width, frame.Height);
    }

    [Fact]
    public void ToByteArray_IsReturnDataOK()
    {
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        var byteArray = frame.ToByteArray();
        var bitmap = GraphicsHelper.GetBgraBitmapFromRawBytes(byteArray, frame.Width, frame.Height);

        bitmap.ShouldBe(frame.Width, frame.Height);
        bitmap.GetPixel(0, 0).ShouldBe(0, 0, 255);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(40)]
    [InlineData(90)]
    [InlineData(100)]
    public void ToByteArray_Jpg_IsReturnDataOK(int quality)
    {
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);

        var byteArray = frame.ToByteArray(ImageFormat.Jpeg(quality));
        var bitmap = SKBitmap.Decode(byteArray);

        bitmap.ShouldBe(frame.Width, frame.Height);
    }

    private static byte[] CreateRedRgbBitmap(int width, int height)
    {
        var bytesPerPixel = 3;
        var bgrData = new byte[width * height * bytesPerPixel];

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
        var targetWidth = 300;
        var targetHeight = 350;

        // Act

        var result = frame.AsRescaledWithPadding(targetWidth, targetHeight);

        // Assert

        result.Should().NotBeNull();
        result.ShouldBe(targetWidth, targetHeight);
    }

    [Fact]
    public void AsRescaledWithPadding_ShouldCenterImage_WithCorrectPadding()
    {
        // Arrange
        var frame = TestFrameHelper.GetTestFrame(TestResources.TestResources.PhotoRed);
        var targetWidth = 300;
        var targetHeight = 350;

        var scale = Math.Min((float)targetWidth / 200, (float)targetHeight / 200);
        var scaledWidth = (int)(200 * scale);
        var scaledHeight = (int)(200 * scale);
        var expectedOffsetX = (targetWidth - scaledWidth) / 2;
        var expectedOffsetY = (targetHeight - scaledHeight) / 2;

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
        var targetWidth = 300;
        var targetHeight = 350;

        // Act
        var result = frame.AsRescaledWithPadding(targetWidth, targetHeight);

        // Assert
        result.GetPixel(0, 0).ShouldBe(0, 0, 0);
        result.GetPixel(targetWidth - 1, targetHeight - 1).ShouldBe(0, 0, 0);
    }
}