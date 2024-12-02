using Deface.NET.Graphics;
using Deface.NET.Graphics.Models;
using Deface.NET.System;
using Deface.NET.UnitTests.Graphics.Helpers;

namespace Deface.NET.UnitTests.Graphics;

public class FrameCreatorUnitTests
{
    private readonly FrameCreator _frameCreator;

    public FrameCreatorUnitTests()
    {
        FileSystem fileSystem = new();
        _frameCreator = new FrameCreator(fileSystem);
    }

    [Fact]
    public void FromFile_ProperImagePath_CreatesFrame()
    {
        var path = TestResources.TestResources.Photo1;

        var frame = _frameCreator.FromFile(path);

        frame.ShouldBe(1280, 946);
    }

    [Fact]
    public void FromFile_InvalidPath_ThrowsDefaceException()
    {
        var path = Guid.NewGuid().ToString();

        var action = () => _frameCreator.FromFile(path);

        action.Should().Throw<DefaceException>();
    }

    [Fact]
    public void FromBgrArray_EmptyArray_ShouldThrowArgumentException()
    {
        var action = () => _frameCreator.FromBgrArray([], 10, 10);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void FromBgrArray_RedImage_FrameIsCorrectAndBlue()
    {
        // Arrange

        var width = 1000;
        var height = 1000;

        var bgrArray = CreateRedBgrImage(1000, 1000);

        // Act

        var result = _frameCreator.FromBgrArray(bgrArray, width, height);

        // Assert

        result.ShouldBe(width, height);

        ShapeTestHelper.ValidateWholeFrame(result, pixel =>
        {
            pixel.ShouldBe(0, 0, 255);
        });
    }

    private static byte[] CreateRedBgrImage(int width, int height)
    {
        int bytesPerPixel = 3;
        int imageSize = width * height * bytesPerPixel;

        byte[] imageData = new byte[imageSize];

        for (int i = 0; i < imageSize; i += bytesPerPixel)
        {
            imageData[i] = 0;     // Blue
            imageData[i + 1] = 0; // Green
            imageData[i + 2] = 255; // Red
        }

        return imageData;
    }
}
